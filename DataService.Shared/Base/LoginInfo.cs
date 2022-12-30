using System;
using System.Collections.Generic;
using System.Text;

namespace DataService.Shared.Base
{
    /// <summary>
    /// 用户登录信息
    /// </summary>
    public class LoginInfo
    {
        /// <summary>
        /// 状态
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// 身份令牌
        /// </summary>
        public string AccessToken { get; set; }
        /// <summary>
        /// 有效期
        /// </summary>
        public double ExpiresIn { get; set; }
        /// <summary>
        /// 身份令牌类型
        /// </summary>
        public string TokenType { get; set; }
        /// <summary>
        /// 基本信息
        /// </summary>
        public ClientInformation BasicInfo { get; set; }
        /// <summary>
        /// 是否需要重置密码
        /// </summary>
        public bool ResetPwd { get; set; } = false;

    }
}
