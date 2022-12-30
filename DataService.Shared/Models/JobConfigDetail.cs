using System;
using System.Collections.Generic;
using System.Text;

namespace DataService.Shared.Models
{
    /// <summary>
    /// 描述：任务详细配置
    /// 作者：Leopisces
    /// 创建日期：2022/8/9 14:24:03
    /// 版本：v1.0
    /// =============================================================
    /// 历史更新记录
    /// 版本：v1.0      
    /// 修改人：
    /// 修改日期：
    /// 修改内容：
    /// ==============================================================
    /// </summary>
    public class JobConfigDetail
    {
        /// <summary>
        /// 任务Key
        /// </summary>
        public string JobKey { get; set; }
        /// <summary>
        /// 任务组
        /// </summary>
        public string Group { get; set; }
        /// <summary>
        /// 执行计划 [cron表达式]
        /// </summary>
        public string Interval { get; set; }
        /// <summary>
        /// 任务开关
        /// </summary>
        public bool Enabled { get; set; }
    }
}
