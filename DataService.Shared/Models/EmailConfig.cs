using System;
using System.Collections.Generic;
using System.Text;

namespace DataService.Shared.Models
{
    /// <summary>
    /// 描述：
    /// 作者：Leopisces
    /// 创建日期：2022/8/3 14:35:00
    /// 版本：v1.0
    /// =============================================================
    /// 历史更新记录
    /// 版本：v1.0      
    /// 修改人：
    /// 修改日期：
    /// 修改内容：
    /// ==============================================================
    public class EmailConfig
    {
        public string AddressFrom { get; set; }
        public string Password { get; set; }
        public string AddressTo { get; set; }
        public string Host { get; set; }
    }
}
