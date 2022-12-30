using System;
using System.Collections.Generic;
using System.Text;

namespace DataService.Dto.BaseInfo
{
    /// <summary>
    /// 描述：用户登录信息
    /// 作者：Leopisces
    /// 创建日期：2022/8/16 16:35:20
    /// 版本：v1.0
    /// =============================================================
    /// 历史更新记录
    /// 版本：v1.0      
    /// 修改人：
    /// 修改日期：
    /// 修改内容：
    /// ==============================================================
    /// </summary>
    /// <summary>
    /// 用户登录信息
    /// </summary>
    public class LoginInfoDto
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
        public ClientInformationDto BasicInfo { get; set; }
        /// <summary>
        /// 是否需要重置密码
        /// </summary>
        public bool ResetPwd { get; set; } = false;

    }
}
