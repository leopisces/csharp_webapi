using DataService.Auto;
using DataService.SqlSugarOrm;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;

namespace Lp.AutoCreate
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //自动创建逻辑
            AutoCreateMethod();
        }

        private static void AutoCreateMethod()
        {
            //编码注册
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            //配置文件
            var builder = new ConfigurationBuilder()
                //.SetBasePath(Directory.GetCurrentDirectory())
                .SetBasePath(Directory.GetCurrentDirectory() + "../../../../")
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            IConfigurationRoot configuration = builder.Build();

            //依赖注入
            IServiceCollection services = new ServiceCollection();
            services.AddOptions();

            //注入SqlSugar配置
            services.Configure<SugarOption>(configuration.GetSection("Sugar"));
            services.Configure<AutoConfig>(configuration.GetSection("AutoConfig"));

            //自动生成model
            services.AddSingleton<AutoCreate>();

            //注入IConfiguration
            services.AddSingleton<IConfiguration>(configuration);

            services.AddLogging();

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            //自动生成
            var autoCreate = serviceProvider.GetService<AutoCreate>();
            autoCreate
                .CreateModel()
                .CreateInterface()
                .CreateImp()
                      //.CreateTableSql("UPL_CHECK_POST_FEEDBACK")
            ;
            Console.WriteLine("执行成功!");
            Console.ReadKey();
        }
    }
}