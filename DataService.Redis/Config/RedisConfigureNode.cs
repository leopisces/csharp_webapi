using System;
using System.Collections.Generic;
using System.Text;

namespace DataService.Redis.Config
{
    /// <summary>
    /// Redis配置节点
    /// </summary>
    public class RedisConfigureNode
    {
        /// <summary>
        /// 数据库编号
        /// </summary>
        public int RedisIndex { get; set; }

        /// <summary>
        /// KEY结构
        /// </summary>
        public string RedisKey { get; set; }

        /// <summary>
        /// 缓存分钟
        /// </summary>
        public int CatchTimeMinite { get; set; } = 5;
    }
}
