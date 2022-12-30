using System;
using System.Collections.Generic;
using System.Text;

namespace DataService.Shared.Exceptions
{
    /// <summary>
    /// 接口访问权限异常
    /// </summary>
    public class AuthException : Exception
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="errmsg"></param>
        public AuthException(string errmsg) : base(errmsg)
        {
        }
    }
}
