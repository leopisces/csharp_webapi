using System;
using System.Collections.Generic;
using System.Text;

namespace DataService.Mongo.Models
{
    /// <summary>
    /// Mongo配置文件
    /// </summary>
    public class MongoConfig
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; }
        /// <summary>
        /// 数据库
        /// </summary>
        public string Database { get; set; }
    }
}
