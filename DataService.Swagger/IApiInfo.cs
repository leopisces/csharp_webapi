using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DataService.Swagger
{
    /// <summary>
    /// API信息接口
    /// </summary>
    public interface IApiInfo
    {
        /// <summary>
        /// 标题
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// 应用程序信息
        /// </summary>
        Assembly ApplicationAssembly { get; }

        /// <summary>
        /// 名称
        /// </summary>
        string ApiName { get; }

        /// <summary>
        /// 版本号
        /// </summary>
        string Version { get; }

        /// <summary>
        /// 描述
        /// </summary>
        string Description { get; }
    }
}
