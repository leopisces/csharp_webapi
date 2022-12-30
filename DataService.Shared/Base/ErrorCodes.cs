using System;
using System.Collections.Generic;
using System.Text;

namespace DataService.Shared.Base
{
    /// <summary>
    /// 错误码
    /// </summary>
    public enum ErrorCodes
    {
        /// <summary>
        /// 请求成功
        /// </summary>
        SUCCESS = 200,
        /// <summary>
        /// 请求错误
        /// </summary>
        REQUEST_ERROR = 400,
        /// <summary>
        /// 登录异常
        /// </summary>
        LOGIN_ERROR = 401,
        /// <summary>
        /// 无访问权限
        /// </summary>
        AUTH_ERROR = 402,
        /// <summary>
        /// Token异常
        /// </summary>
        TOKEN_ERROR = 403,
        /// <summary>
        /// 参数错误
        /// </summary>
        PARAMS_ERROR = 422,
        /// <summary>
        /// 服务器错误
        /// </summary>
        SERVE_ERROR = 500,


    }
}
