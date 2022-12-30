using System;
using System.Collections.Generic;
using System.Text;

namespace DataService.Shared.Models
{
    /// <summary>
    /// 描述：后台定时任务
    /// 作者：Leopisces
    /// 创建日期：2022/8/9 15:23:07
    /// 版本：v1.0
    /// =============================================================
    /// 历史更新记录
    /// 版本：v1.0      
    /// 修改人：
    /// 修改日期：
    /// 修改内容：
    /// ==============================================================
    /// </summary>
    public class BackJobs
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        public string JobName { get; set; }

        /// <summary>
        /// cron表达式
        /// </summary>
        public string Interval { get; set; }

        /// <summary>
        /// 是否开启
        /// </summary>
        public bool Enabled { get; set; }
    }
}
