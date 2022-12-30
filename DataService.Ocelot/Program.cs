using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Ocelot.Provider.Consul;

namespace DataService.Ocelot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                              .ConfigureKestrel((Context, options) =>
                              {
                                  options.Limits.KeepAliveTimeout = TimeSpan.FromMilliseconds(800);
                                  options.AllowSynchronousIO = true;

                                  options.Listen(IPAddress.Any, 10003, listenOptions =>
                                  {
                                      listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                                      listenOptions.UseConnectionLogging();
                                  });
                              });
                })
            ;
    }
}
