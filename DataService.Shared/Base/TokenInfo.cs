using System;
using System.Collections.Generic;
using System.Text;

namespace DataService.Shared.Base
{
    /// <summary>
    /// Token信息
    /// </summary>
    public class TokenInfo
    {
        /// <summary>
        /// 状态
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// Token
        /// </summary>
        public string Access_Token { get; set; }
        /// <summary>
        /// 有效期
        /// </summary>
        public double Expires_In { get; set; }
        /// <summary>
        /// 身份令牌类型
        /// </summary>
        public string Token_Type { get; set; }
    }
}
