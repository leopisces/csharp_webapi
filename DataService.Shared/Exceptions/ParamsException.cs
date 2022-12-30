using System;
using System.Collections.Generic;
using System.Text;

namespace DataService.Domain.Shared.Exceptions
{
    /// <summary>
    /// 参数异常
    /// </summary>
    public class ParamsException : Exception
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="errmsg"></param>
        public ParamsException(string errmsg) : base(errmsg)
        {
        }
    }

}
