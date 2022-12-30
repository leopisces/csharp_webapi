using DataService.Shared.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataService.HostApi.Jobs
{
    /// <summary>
    /// 描述：Quartz定时任务
    /// 作者：Leopisces
    /// 创建日期：2022/8/9 13:54:29
    /// 版本：v1.0
    /// =============================================================
    /// 历史更新记录
    /// 版本：v1.0      
    /// 修改人：
    /// 修改日期：
    /// 修改内容：
    /// ==============================================================
    /// </summary>
    public class QuartzTestJob : IJob
    {
        private ILogger<QuartzTestJob> _logger;

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task Execute(IJobExecutionContext context)
        {
            //取传递的参数
            var locker = context.Trigger.JobDataMap.Get("locker");

            //Quartz不能使用注入
            _logger = DIContainer.ServiceLocator.Instance.GetService<ILogger<QuartzTestJob>>();
            _logger.LogInformation("我是Quartz任务!");
            return Task.CompletedTask;
        }
    }
}