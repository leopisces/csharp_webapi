using System;
using System.Collections.Generic;
using System.Text;

namespace DataService.Shared.Exceptions
{
    /// <summary>
    /// 登录异常
    /// </summary>
    public class LoginException : Exception
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="errmsg"></param>
        public LoginException(string errmsg) : base(errmsg)
        {
        }
    }

}
