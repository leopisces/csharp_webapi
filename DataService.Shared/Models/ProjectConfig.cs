using System;
using System.Collections.Generic;
using System.Text;

namespace DataService.Shared.Models
{
    /// <summary>
    /// 项目配置
    /// </summary>
    public class ProjectConfig
    {
        /// <summary>
        /// Url权限
        /// </summary>
        public bool UrlAuth { get; set; }
        /// <summary>
        /// 允许跨越的ip
        /// </summary>
        public string CorsOrigins { get; set; }
    }
}
