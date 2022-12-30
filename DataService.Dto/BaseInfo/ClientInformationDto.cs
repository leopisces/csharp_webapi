using DataService.Shared.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataService.Dto.BaseInfo
{
    /// <summary>
    /// 描述：登录用户信息
    /// 作者：Leopisces
    /// 创建日期：2022/8/16 16:37:11
    /// 版本：v1.0
    /// =============================================================
    /// 历史更新记录
    /// 版本：v1.0      
    /// 修改人：
    /// 修改日期：
    /// 修改内容：
    /// ==============================================================
    /// </summary>
    public class ClientInformationDto : IClientInformation
    {
        /// <summary>
        /// id
        /// </summary>
        public decimal? ID { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }
    }
}
