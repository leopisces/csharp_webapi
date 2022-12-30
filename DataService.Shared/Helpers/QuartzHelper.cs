using DataService.Shared.Base;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Shared.Helpers
{
    /// <summary>
    /// 描述：
    /// 作者：Leopisces
    /// 创建日期：2022/8/3 14:29:59
    /// 版本：v1.0
    /// =============================================================
    /// 历史更新记录
    /// 版本：v1.0      
    /// 修改人：
    /// 修改日期：
    /// 修改内容：
    /// ==============================================================
    public static class QuartzHelper
    {
        private static ISchedulerFactory _schedulerFactory;
        private static IScheduler _scheduler;

        public static async Task Start()
        {
            Init();
            _scheduler = await _schedulerFactory.GetScheduler();
            await _scheduler.Start();
        }

        /// <summary>
        /// 获取任务执行状态
        /// </summary>
        /// <param name="jobKey"></param>
        /// <returns></returns>
        public static async Task<TriggerState> Status(JobKey jobKey)
        {
            var r = _scheduler.GetTriggersOfJob(jobKey).Result;
            if (r != null && r.Count > 0)
            {
                return await _scheduler.GetTriggerState(r.ToList().FirstOrDefault().Key);
            }
            return TriggerState.Error;
        }

        /// <summary>
        /// 判断任务是否存在
        /// </summary>
        /// <param name="jobKey"></param>
        /// <param name="logClient"></param>
        /// <returns></returns>
        public static async Task<bool> IsExists(JobKey jobKey)
        {
            IJobDetail r = null;
            if (_scheduler != null)
            {
                r = await _scheduler.GetJobDetail(jobKey);
            }
            return r == null ? false : true;
        }

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="type">类</param>
        /// <param name="jobKey">键</param>
        /// <param name="trigger">触发器</param>
        public static async Task Add(Type type, JobKey jobKey, ITrigger trigger = null)
        {
            if (_scheduler == null)
            {
                await Start();
            }

            if (trigger == null)
            {
                trigger = TriggerBuilder.Create()
                    .WithIdentity("april.trigger")
                    .WithDescription("default")
                    .WithSimpleSchedule(x => x.WithMisfireHandlingInstructionFireNow().WithRepeatCount(-1))
                    .Build();
            }
            var job = JobBuilder.Create(type)
                .WithIdentity(jobKey)
                .Build();

            await _scheduler.ScheduleJob(job, trigger);
        }
        /// <summary>
        /// 恢复任务
        /// </summary>
        /// <param name="jobKey">键</param>
        public static async Task Resume(JobKey jobKey)
        {
            if (_scheduler == null)
            {
                await Start();
            }
            //LogHelper.WriteInfo(typeof(QuartzHelper), $"恢复任务{jobKey.Group},{jobKey.Name}");
            await _scheduler.ResumeJob(jobKey);
        }
        /// <summary>
        /// 停止任务
        /// </summary>
        /// <param name="jobKey">键</param>
        public static async Task Stop(JobKey jobKey)
        {
            if (_scheduler == null)
            {
                await Start();
            }
            //LogHelper.WriteInfo(typeof(QuartzHelper), $"暂停任务{jobKey.Group},{jobKey.Name}");
            await _scheduler.PauseJob(jobKey);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private static void Init()
        {
            if (_schedulerFactory == null)
            {
                _schedulerFactory = DIContainer.ServiceLocator.Instance.GetService<ISchedulerFactory>();
            }
        }
    }
}
