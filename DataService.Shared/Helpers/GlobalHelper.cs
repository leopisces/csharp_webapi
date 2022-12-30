using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DataService.Shared.Helpers
{
    /// <summary>
    /// 描述：log4net日志类
    /// 作者：Leopisces
    /// 创建日期：2022/8/5 22:52:53
    /// 版本：v1.0
    /// =============================================================
    /// 历史更新记录
    /// 版本：v1.1     
    /// 修改人：
    /// 修改日期：
    /// 修改内容：
    /// ==============================================================
    public class GlobalHelper
    {
        /// <summary>
        /// 数据层接口的注入
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="prefix">实现的接口约定的前缀</param>
        /// <param name="endwith">实现的接口约定的后缀</param>
        /// <returns></returns>
        public static Dictionary<Type, Type> GetInterfaceAndImp(string assemblyName, string prefix, string endwith)
        {
            if (!string.IsNullOrEmpty(assemblyName))
            {
                List<Type> ts = Assembly.Load(assemblyName).GetTypes().ToList();
                //获取当前程序集
                //List<Type> ts = Assembly.GetEntryAssembly().GetTypes().ToList();
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
                    if (!item.FullName.ToUpper().StartsWith(assemblyName.ToUpper()))
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
                        LogHelper.Log(LogLevel.Information, MessageHelper.INTERFACEANDIMPNOTFOUND);
                    }
                }
                return result;
            }
            return new Dictionary<Type, Type>();
        }

    }
}
