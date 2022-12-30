using System;
using System.Collections.Generic;
using System.Text;

namespace DataService.Shared.Exceptions
{
    /// <summary>
    /// Token异常
    /// </summary>
    public class TokenException : Exception
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="errmsg"></param>
        public TokenException(string errmsg) : base(errmsg)
        {
        }
    }
}
