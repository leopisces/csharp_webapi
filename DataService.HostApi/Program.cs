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
        /// �������
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
                //����Elasticsearch��Kibana(EK8.0�汾�Ժ�Serilog.Extensions.Hosting����ʱ��֧��)
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

                        //AgileConfig ��һ�ַ�ʽ
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
                        //����log4net
                        //builder.SetMinimumLevel(LogLevel.Trace);
                        //builder.AddLog4Net("log4net.config");

                        //����Nlog
                        //��������е���־��¼�ṩ����
                        builder.ClearProviders();
                        //ʹ��΢���ṩ�Ŀ���̨���
                        builder.AddConsole();
                        //����NLog
                        builder.AddNLog();
                        //��NLog�е���־��������������,�������е���־����ת����NLog,Ȼ����NLog���ж��ι���
                        builder.AddFilter<NLogLoggerProvider>(level => level >= LogLevel.Trace);
                    });
                })
              ;
    }
}
