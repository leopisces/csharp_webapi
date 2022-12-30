using System;
using System.Collections.Generic;
using System.Text;

namespace DataService.Shared.Helpers
{
    /// <summary>
    /// 公共消息
    /// </summary>
    public sealed class MessageHelper
    {
        /// <summary>
        /// Redis缓存失败
        /// </summary>
        public const string REDIS_CACHE_FAILD = "Redis缓存失败!";
        /// <summary>
        /// 账号密码错误
        /// </summary>
        public const string ACCOUNT_OR_PASSWORD_ERROR = "账号密码错误!";
        /// <summary>
        /// 该账号,系统不存在
        /// </summary>
        public const string ACCOUNT_NOT_EXISTS = "该账号,系统不存在!";
        /// <summary>
        /// 未找到符合的接口和实现类
        /// </summary>
        public const string INTERFACEANDIMPNOTFOUND = "未找到符合的接口和实现类";
    }
}
