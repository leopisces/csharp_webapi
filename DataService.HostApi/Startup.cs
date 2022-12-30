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
    /// ����
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// �����ļ�
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// ���캯��
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
                    //�� Controller ʹ���Դ�����
                    .AddControllersAsServices();
            //ע��API��Ϣ
            services.AddSingleton(ApiInfo.Instantiate(Configuration, GetType().Assembly));
            //ע��swagger
            services.AddCustomSwagger(ApiInfo.Instance);
            //swagger����ĵ����
            services.AddScoped<SpireDocHelper>();
            services.AddScoped<SwaggerGenerator>(); //ע��SwaggerGenerator,�������ֱ��ʹ���������

            //·��Сд
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddMvc(options =>
                    {
                        //ͳһ��ʽ����
                        options.Filters.Add(typeof(ApiResultFilterAttribute));
                        //ͳһ�쳣����
                        options.Filters.Add(typeof(CustomExceptionAttribute));
                    })
                    .AddNewtonsoftJson(options =>
                    {
                        //��ʹ���շ���ʽ��key
                        options.SerializerSettings.ContractResolver = new ToLowerContractResolver();
                        //options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    });
            //ע��Sugar���ݿ�����
            services.Configure<SugarOption>(Configuration.GetSection("Sugar"));

            //ע�������������
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //ע��JWT
            services.AddJwt(Configuration);

            //ע��Redis
            services.Configure<RedisOption>(Configuration.GetSection("RedisConfig"));
            services.AddSingleton<IRedisService, RedisService>();

            //ע���̨��ʱ����
            services.AddSingleton<IHostedService, TestBackGroudService>();
            services.Configure<TimingConfig>(Configuration.GetSection("TimingConfig"));
            services.TryAddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddSingleton<TimingManager>();

            //ע��Hangfire��̨��ʱ����
            services.AddHangfire(options =>
            {
                options.UseStorage(new RedisStorage(
                    Configuration.GetSection("Hangfire:ConnectionString").Value,
                    new RedisStorageOptions
                    {
                        // redis��DB��
                        Db = Convert.ToInt32(Configuration.GetSection("Hangfire:db").Value),
                        // �����ö�ȡһ�Σ����Ϊ15�룬���Դ˴����õ���15��Ҳ�ǻ�Ĭ��Ϊ15��
                        FetchTimeout = TimeSpan.FromMilliseconds(10),
                    }));
            });
            services.AddHangfireServer(options =>
            {
                options.Queues = new[] { "alpha", "beta", "default" };
            });

            //ע��SignalR
            services.AddSignalR();
            services.AddSingleton<IServerHubImp, ServerHubImp>();

            //ע��ILogger
            //services.AddLogging(builder =>
            //{
            //    //����Դ�����־ʹ��log4net�����ļ� ConsoleAppender ��
            //    builder.ClearProviders();
            //    //�Զ����ILogger
            //    //builder.AddProvider(new LogProvider());
            //});

            //���ÿ���������������Դ��
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

            // ע����ȨHandler
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

            //α����ע��
            services.Replace(ServiceDescriptor.Transient<IControllerActivator, XcServiceBasedControllerActivator>());

            //ע��Mongodb
            services.Configure<MongoConfig>(Configuration.GetSection("MongoConfig"));
            services.AddSingleton<MongoDbContext>();
            services.AddSingleton<IBaseMongoRepository, BaseMongoRepository>();

            // ��ӽ������
            services.AddHealthChecks();
            // ע��Consul����
            services.RegisterConsulServices(Configuration.GetSection("ServiceConfig").Get<ServiceConfig>());

            //ע��ҵ����������ݽӿں�ʵ����
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
                //ָ��ʵ������·��
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
                OnPrepareResponse = (c) =>
                {
                    c.Context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                },
                //����URL������ļ�·��
                RequestPath = new PathString("/wwwroot")
            });

            //app.UseHangfireServer();

            //����֧��
            app.UseCors("CorsPolicy");

            app.UseHangfireDashboard("/hangfire", new Hangfire.DashboardOptions
            {
                IgnoreAntiforgeryToken = true,
                DashboardTitle = "Hangfire���ҳ��",
                Authorization = new[] { new HangfireFilter() },
                IsReadOnlyFunc = (DashboardContext context) => true  //  ����Ϊֻ��
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
