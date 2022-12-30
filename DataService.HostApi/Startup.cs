using DataService.Shared.Base;
using DataService.Domain.Shared.Filters;
using DataService.SqlSugarOrm;
using DataService.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DataService.Domain.Shared.Json;
using Microsoft.AspNetCore.Http;
using DataService.JWT;
using DataService.Redis;
using System;
using System.Linq;
using DataService.Shared.Helpers.Config;
using System.IO;
using Microsoft.Extensions.FileProviders;
using DataService.HostApi.TaskServices;
using Microsoft.Extensions.Logging;
using DataService.Shared.Models;
using Quartz;
using Quartz.Impl;
using DataService.Shared.Managers;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Hangfire;
using Hangfire.Redis;
using Hangfire.Dashboard;
using DataService.Shared.Filters;
using DataService.HostApi.Socket;
using Microsoft.AspNetCore.Authorization;
using DataService.HostApi.Handlers;
using DataService.HostApi.AutoMapper;
using DotNetCore.CAP;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using Microsoft.AspNetCore.Mvc.Controllers;
using DataService.HostApi.Acivators;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using DataService.Mongo.Models;
using DataService.Mongo.IRepository;
using DataService.Mongo.Repository;
using DataService.Mongo.Temp;
using DataService.Consul;

namespace DataService.HostApi
{
    /// <summary>
    /// 配置
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 配置文件
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                    //让 Controller 使用自带容器
                    .AddControllersAsServices();
            //注入API信息
            services.AddSingleton(ApiInfo.Instantiate(Configuration, GetType().Assembly));
            //注入swagger
            services.AddCustomSwagger(ApiInfo.Instance);
            //swagger输出文档相关
            services.AddScoped<SpireDocHelper>();
            services.AddScoped<SwaggerGenerator>(); //注入SwaggerGenerator,后面可以直接使用这个方法

            //路由小写
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddMvc(options =>
                    {
                        //统一格式处理
                        options.Filters.Add(typeof(ApiResultFilterAttribute));
                        //统一异常处理
                        options.Filters.Add(typeof(CustomExceptionAttribute));
                    })
                    .AddNewtonsoftJson(options =>
                    {
                        //不使用驼峰样式的key
                        options.SerializerSettings.ContractResolver = new ToLowerContractResolver();
                        //options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    });
            //注入Sugar数据库配置
            services.Configure<SugarOption>(Configuration.GetSection("Sugar"));

            //注入请求的上下文
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //注入JWT
            services.AddJwt(Configuration);

            //注入Redis
            services.Configure<RedisOption>(Configuration.GetSection("RedisConfig"));
            services.AddSingleton<IRedisService, RedisService>();

            //注入后台定时服务
            services.AddSingleton<IHostedService, TestBackGroudService>();
            services.Configure<TimingConfig>(Configuration.GetSection("TimingConfig"));
            services.TryAddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddSingleton<TimingManager>();

            //注入Hangfire后台定时任务
            services.AddHangfire(options =>
            {
                options.UseStorage(new RedisStorage(
                    Configuration.GetSection("Hangfire:ConnectionString").Value,
                    new RedisStorageOptions
                    {
                        // redis的DB区
                        Db = Convert.ToInt32(Configuration.GetSection("Hangfire:db").Value),
                        // 间隔多久读取一次，最低为15秒，所以此处设置低于15秒也是会默认为15秒
                        FetchTimeout = TimeSpan.FromMilliseconds(10),
                    }));
            });
            services.AddHangfireServer(options =>
            {
                options.Queues = new[] { "alpha", "beta", "default" };
            });

            //注入SignalR
            services.AddSignalR();
            services.AddSingleton<IServerHubImp, ServerHubImp>();

            //注入ILogger
            //services.AddLogging(builder =>
            //{
            //    //清除自带的日志使用log4net配置文件 ConsoleAppender 的
            //    builder.ClearProviders();
            //    //自定义的ILogger
            //    //builder.AddProvider(new LogProvider());
            //});

            //配置跨域处理，允许所有来源：
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    policy => policy.AllowAnyOrigin()
                                    .AllowAnyHeader()
                                    .AllowAnyMethod());
            });
            //services.AddCors(options =>
            //    options.AddPolicy("CorsPolicy", p =>
            //        p.WithOrigins(
            //                Configuration.GetSection("Project:CorsOrigins").Value
            //            .Split(",", StringSplitOptions.RemoveEmptyEntries)
            //            .ToArray())
            //        .AllowAnyHeader()
            //        .AllowAnyMethod()
            //        .AllowCredentials())
            //);

            // 注入授权Handler
            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();

            //AutoMapper
            services.AddAutoMapper(typeof(UserProfile));

            //CAP
            services.AddCap(x =>
            {
                x.UsePostgreSql(Configuration.GetSection("Cap:PostgreSql").Value);
                x.UseDashboard(x => x.PathMatch = "/caps");
                x.UseRabbitMQ(rb =>
                {
                    rb.HostName = Configuration.GetSection("RabbitMq:HostName").Value;
                    rb.UserName = Configuration.GetSection("RabbitMq:UserName").Value;
                    rb.Password = Configuration.GetSection("RabbitMq:Password").Value;
                    rb.Port = Convert.ToInt32(Configuration.GetSection("RabbitMq:Port").Value);
                    rb.ExchangeName = Configuration.GetSection("RabbitMq:ExchangeName").Value;
                });
                x.SucceedMessageExpiredAfter = 24 * 3600;

                x.FailedRetryCount = 5;
            });

            //伪属性注入
            services.Replace(ServiceDescriptor.Transient<IControllerActivator, XcServiceBasedControllerActivator>());

            //注入Mongodb
            services.Configure<MongoConfig>(Configuration.GetSection("MongoConfig"));
            services.AddSingleton<MongoDbContext>();
            services.AddSingleton<IBaseMongoRepository, BaseMongoRepository>();

            // 添加健康检查
            services.AddHealthChecks();
            // 注入Consul服务
            services.RegisterConsulServices(Configuration.GetSection("ServiceConfig").Get<ServiceConfig>());

            //注入业务层所有数据接口和实现类
            ConfigHelper
                .GetAssemblyName("DataService.Application", "I", "Imp")
                .ToList()
                .ForEach(item =>
                {
                    services.AddTransient(item.Value, item.Key);
                });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="applicationLifetime"></param>
        /// <param name="apiInfo"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,IHostApplicationLifetime applicationLifetime, IApiInfo apiInfo)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseDirectoryBrowser();

            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")))
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"));
            }
            app.UseStaticFiles(new StaticFileOptions
            {
                //指定实际物理路径
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
                OnPrepareResponse = (c) =>
                {
                    c.Context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                },
                //设置URL请求的文件路径
                RequestPath = new PathString("/wwwroot")
            });

            //app.UseHangfireServer();

            //跨域支持
            app.UseCors("CorsPolicy");

            app.UseHangfireDashboard("/hangfire", new Hangfire.DashboardOptions
            {
                IgnoreAntiforgeryToken = true,
                DashboardTitle = "Hangfire监控页面",
                Authorization = new[] { new HangfireFilter() },
                IsReadOnlyFunc = (DashboardContext context) => true  //  设置为只读
            });

            app.UseCustomSwagger(apiInfo);

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseCapDashboard();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/Health");
                endpoints.MapControllers();
                endpoints.MapHub<ServerHubImp>("/serverHub");
            });

            DIContainer.ServiceLocator.Instance = app.ApplicationServices;
        }
    }
}
