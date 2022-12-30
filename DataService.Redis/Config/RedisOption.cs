﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DataService.Redis
{
    public class RedisOption
    {
        /// <summary>
        /// Redis服务器 
        /// </summary>
        public string RedisConnectionString { get; set; }

        /// <summary>
        /// Redis密码
        /// </summary>
        public string RedisConnectionPwd { get; set; }

        /// <summary>
        /// 系统自定义Key前缀
        /// </summary>
        public string RedisPrefixKey { get; set; }
    }
}
