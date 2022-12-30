using System;
using System.Collections.Generic;
using System.Text;

namespace DataService.Swagger
{
    public enum GroupVersion
    {
        /// <summary>
        /// WebApi
        /// </summary>
        [GroupInfo(Title = "WebApi", Description = "接口文档", Version = "v1.0.0")]
        WEBAPI = 1,
        /// <summary>
        /// ApiTest
        /// </summary>
        [GroupInfo(Title = "ApiTest", Description = "接口测试", Version = "v1.0.0")]
        ApiTest,
    }
}
