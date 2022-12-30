using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DataService.Shared.Helpers.Config
{
    /// <summary>
    /// 描述：配置工具类
    /// 作者：Leopisces
    /// 创建日期：2022/8/1 19:55:35
    /// 版本：v1.1
    /// =============================================================
    /// 历史更新记录
    /// 版本：v1.1      
    /// 修改人：Leopisces
    /// 修改日期：
    /// 修改内容：
    /// ==============================================================
    public class ConfigHelper
    {
        /// <summary>
        /// 获取程序集中的实现类对应的多个接口
        /// </summary>
        /// <returns>The class name.</returns>
        /// <param name="className">class name.</param>
        /// <param name="prefix">Prefix.</param>
        /// <param name="endwith"></param>
        public static Dictionary<Type, Type> GetClassName(string className, string prefix, string endwith)
        {
            if (!string.IsNullOrEmpty(className))
            {
                Assembly assembly = Assembly.GetEntryAssembly();
                List<Type> ts = assembly.GetTypes().ToList();

                var result = new Dictionary<Type, Type>();
                foreach (var item in ts.Where(s => !s.IsInterface))
                {

                    if (item.Name == className + "`")
                    {
                        continue;
                    }

                    if (!item.Name.ToUpper().EndsWith(endwith.ToUpper()))
                    {
                        continue;
                    }

                    string itemName = prefix + item.Name;

                    var interfaceTypes = item.GetInterfaces();

                    bool isResistory = true; //interfaceTypes.Any(c => c.Name == "IBusinessRepository`1");

                    var interfaceType = interfaceTypes.Where(c => c.Name == itemName).FirstOrDefault();
                    if (interfaceType != null && isResistory)
                    {
                        result.Add(item, interfaceType);
                    }
                    else
                    {
                        LogHelper.Log(LogLevel.Trace, "未获取程序集中的实现类对应的多个接口");
                    }

                }
                return result;
            }
            return new Dictionary<Type, Type>();
        }

        /// <summary>
        /// 获取程序集中的实现类对应的多个接口
        /// </summary>
        /// <returns>The class name.</returns>
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="prefix">接口类的前缀</param>
        /// <param name="endwith">接口类的后缀</param>
        public static Dictionary<Type, Type> GetAssemblyName(string assemblyName, string prefix, string endwith)
        {
            if (!string.IsNullOrEmpty(assemblyName))
            {
                Assembly assembly = Assembly.Load(assemblyName);
                List<Type> ts = assembly.GetTypes().ToList();

                var result = new Dictionary<Type, Type>();
                foreach (var item in ts.Where(s => !s.IsInterface))
                {
                    if (item.Name == assemblyName + "`")
                    {
                        continue;
                    }

                    if (!item.Name.ToUpper().EndsWith(endwith.ToUpper()))
                    {
                        continue;
                    }

                    string itemName = prefix + item.Name;

                    var interfaceTypes = item.GetInterfaces();

                    bool isResistory = true; //interfaceTypes.Any(c => c.Name == "IBusinessRepository`1");

                    var interfaceType = interfaceTypes.Where(c => c.Name == itemName).FirstOrDefault();
                    if (interfaceType != null && isResistory)
                    {
                        result.Add(item, interfaceType);
                    }
                    else
                    {
                        LogHelper.Log(LogLevel.Trace, "未获取当前程序的实现类对应的多个接口");
                    }

                }
                return result;
            }
            return new Dictionary<Type, Type>();
        }

        /// <summary>
        /// 加载Configuration配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetAppSettings<T>(string key) where T : class, new()
        {
            IConfiguration config = new ConfigurationBuilder()
                .Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true })
                .Build();

            T appconfig = new ServiceCollection()
                .AddOptions()
                .Configure<T>(config.GetSection(key))
                .BuildServiceProvider()
                .GetService<IOptions<T>>()
                .Value;

            return appconfig;
        }
    }
}
