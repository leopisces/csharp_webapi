using System;
using System.Collections.Generic;
using System.Text;

namespace DataService.Shared.Base
{
    /// <summary>
    /// 用户基础信息
    /// </summary>
    public interface IClientInformation
    {
        /// <summary>
        /// 系统编码
        /// </summary>
        decimal? ID { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        string Account { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        string Phone { get; set; }
    }
}
