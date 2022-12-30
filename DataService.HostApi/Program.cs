using Com.Ctrip.Framework.Apollo;
using Com.Ctrip.Framework.Apollo.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Net;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using Serilog.Events;
using AgileConfig.Client;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;

namespace DataService.HostApi
{
    /// <summary>
    /// Program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 程序入口
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception exception)
            {
                //NLog: catch setup errors
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }

            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// CreateHost
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //集成Elasticsearch与Kibana(EK8.0版本以后Serilog.Extensions.Hosting库暂时不支持)
                //.UseSerilog((ctx, config) =>
                //{
                //    config.MinimumLevel.Information()
                //          .Enrich.FromLogContext()
                //          .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://es.leopisces.cn"))
                //          {
                //              IndexFormat = "webapi-logs-{0:yyyy.MM.dd}",
                //              MinimumLogEventLevel = LogEventLevel.Information,
                //              EmitEventFailure = EmitEventFailureHandling.RaiseCallback,
                //              FailureCallback = e => Console.WriteLine("Unable to submit event " + e.MessageTemplate),
                //              AutoRegisterTemplate = true,
                //              AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                //              ModifyConnectionSettings =
                //                conn =>
                //                {
                //                    conn.ServerCertificateValidationCallback((source, certificate, chain, sslPolicyErrors) => true);
                //                    conn.BasicAuthentication("elastic", "123456");
                //                    return conn;
                //                }
                //          })
                //          .WriteTo.Console();
                //})
                //.UseAgileConfig(new ConfigClient($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json"), e => Console.WriteLine($"Action={e.Action},Key={e.Key}"))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((hostingContext, builder) =>
                    {
                        //LogManager.UseConsoleLogging(Com.Ctrip.Framework.Apollo.Logging.LogLevel.Trace);
                        builder.AddApollo(builder.Build().GetSection("Apollo"))
                               .AddDefault()
                               .AddNamespace("webapi", ConfigFileFormat.Json);

                        //AgileConfig 另一种方式
                        //var envName = hostingContext.HostingEnvironment.EnvironmentName;
                        //var configClient = new ConfigClient($"appsettings.{envName}.json");
                        //builder.AddAgileConfig(configClient, arg => Console.WriteLine($"config changed , action:{arg.Action} key:{arg.Key}"));

                    })
                    .UseStartup<Startup>()
                    .ConfigureKestrel((Context, options) =>
                    {
                        options.Limits.KeepAliveTimeout = TimeSpan.FromMilliseconds(800);
                        options.AllowSynchronousIO = true;

                        var port = Context.Configuration.GetSection("Project:Port").Value;

                        options.Listen(IPAddress.Any, int.Parse(port), listenOptions =>
                        {
                            listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                            listenOptions.UseConnectionLogging();
                        });
                    })
                    .ConfigureLogging(builder => 
                    {
                        //配置log4net
                        //builder.SetMinimumLevel(LogLevel.Trace);
                        //builder.AddLog4Net("log4net.config");

                        //配置Nlog
                        //先清空所有的日志记录提供程序
                        builder.ClearProviders();
                        //使用微软提供的控制台输出
                        builder.AddConsole();
                        //引入NLog
                        builder.AddNLog();
                        //将NLog中的日志输出级别设置最低,这样所有的日志都会转发到NLog,然后由NLog进行二次过滤
                        builder.AddFilter<NLogLoggerProvider>(level => level >= LogLevel.Trace);
                    });
                })
              ;
    }
}
