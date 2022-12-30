using System;
using System.Collections.Generic;
using System.Text;

namespace DataService.Shared.Models
{
    /// <summary>
    /// 描述：定时配置
    /// 作者：Leopisces
    /// 创建日期：2022/8/9 14:22:05
    /// 版本：v1.0
    /// =============================================================
    /// 历史更新记录
    /// 版本：v1.0      
    /// 修改人：
    /// 修改日期：
    /// 修改内容：
    /// ==============================================================
    /// </summary>
    public class TimingConfig
    {
        /// <summary>
        /// 项目配置名称
        /// </summary>
        public string Project { get; set; }
        /// <summary>
        /// 配置
        /// </summary>
        public Dictionary<string, List<JobConfigDetail>> JobConfigs { get; set; }

    }
}
