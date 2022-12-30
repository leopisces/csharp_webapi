using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataService.Swagger
{
    /// <summary>
    /// 系统分组特性
    /// </summary>
    public class ApiGroupAttribute : Attribute, IApiDescriptionGroupNameProvider
    { 
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        public ApiGroupAttribute(GroupVersion name)
        {
            GroupName = name.ToString().ToLower();
        }

        /// <summary>
        /// 分组名称
        /// </summary>
        /// <value></value>
        public string GroupName { get; set; }
    }
}
