using System;
using System.Collections.Generic;
using System.Text;

namespace DataService.Shared.Base
{
    /// <summary>
    /// 登录用户信息
    /// </summary>
    public class ClientInformation : IClientInformation
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
