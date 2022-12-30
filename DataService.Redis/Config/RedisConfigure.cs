using System;
using System.Collections.Generic;
using System.Text;

namespace DataService.Redis.Config
{
    /// <summary>
    /// Redis配置
    /// </summary>
    public class RedisConfigure
    {
        /// <summary>
        /// 用户登录信息
        /// </summary>
        public static RedisConfigureNode USERINFOS = new RedisConfigureNode { RedisIndex = 0, RedisKey = "UserInofs:{0}" };
    }
}
