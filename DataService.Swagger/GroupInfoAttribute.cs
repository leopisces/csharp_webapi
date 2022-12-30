using System;
using System.Collections.Generic;
using System.Text;

namespace DataService.Swagger
{
    /// <summary>
    /// 系统模块枚举注释
    /// </summary>
    public class GroupInfoAttribute : Attribute
    {
        /// <summary>
        /// 接口名称
        /// </summary>
        /// <value></value>
        public string Title { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        /// <value></value>
        public string Version { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        /// <value></value>
        public string Description { get; set; }
    }
}
