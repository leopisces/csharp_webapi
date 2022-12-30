using DataService.Shared.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataService.Shared.Managers
{
    /// <summary>
    /// 描述：
    /// 作者：Leopisces
    /// 创建日期：2022/8/9 14:01:22
    /// 版本：v1.0
    /// =============================================================
    /// 历史更新记录
    /// 版本：v1.0      
    /// 修改人：
    /// 修改日期：
    /// 修改内容：
    /// ==============================================================
    /// </summary>
    public class TimingManager
    {
        private readonly ILogger<TimingManager> _logger;
        private ISchedulerFactory _schedulerFactory;
        private IScheduler _scheduler;
        private object locker = "LOCKME";
        private TimingConfig _config;

        public TimingManager(ILogger<TimingManager> logger, ISchedulerFactory schedulerFactory, IOptions<TimingConfig> options)
        {
            _config = options.Value;
            _schedulerFactory = schedulerFactory;
            _logger = logger;
        }

        public async Task<List<JobKey>> Start<T>() where T : IJob
        {
            return await Task.Run(() =>
            {
                var jobKeys = new List<JobKey>();
                var excuteConfig = _config.JobConfigs[typeof(T).Name];
                if (excuteConfig != null && excuteConfig.Count > 0)
                {
                    excuteConfig.ForEach(item =>
                    {
                        if (item.Enabled)
                        {
                            var trigger = Guid.NewGuid().ToString();
                            _logger.LogInformation($"[任务组:{item.Group} 任务Key:{item.JobKey}]");
                            jobKeys.Add(StartTask<T>(trigger, item.Interval, item.JobKey, item.Group).Result);
                        }
                    });
                }
                return jobKeys;
            });
        }

        /// <summary>
        /// 开始执行
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="type"></param>
        /// <param name="action"></param>
        public async Task<JobKey> StartTask<T>(string triggerName, string action, string jobKey, string jobGroup)
        {
            ITrigger trigger = TriggerBuilder.Create()
               .WithIdentity(triggerName, typeof(TimingManager).Name.ToLower())
               .WithCronSchedule(action)  //cron表达式
                                          //.StartAt(DateTime.Now.AddSeconds(3))
                                          //.WithSimpleSchedule(x => x.WithIntervalInMinutes(10).RepeatForever())
               .Build();
            //传参
            trigger.JobDataMap.Add("locker", locker);
            var key = new JobKey(jobKey, jobGroup);
            await Add(typeof(T), key, trigger);
            Thread.Sleep(1000);
            return key;
        }


        public async Task Start()
        {
            _scheduler = await _schedulerFactory.GetScheduler();
            await _scheduler.Start();
        }

        /// <summary>
        /// 获取任务执行状态
        /// </summary>
        /// <param name="jobKey"></param>
        /// <returns></returns>
        public async Task<TriggerState> Status(JobKey jobKey)
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
        public async Task<bool> IsExists(JobKey jobKey)
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
        public async Task<JobKey> Add(Type type, JobKey jobKey, ITrigger trigger = null)
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
            return jobKey;
        }

        /// <summary>
        /// 恢复任务
        /// </summary>
        /// <param name="jobKey">键</param>
        public async Task Resume(JobKey jobKey)
        {
            if (_scheduler == null)
            {
                await Start();
            }
            await _scheduler.ResumeJob(jobKey);
        }

        /// <summary>
        /// 停止任务
        /// </summary>
        /// <param name="jobKey">键</param>
        public async Task Stop(JobKey jobKey)
        {
            if (_scheduler == null)
            {
                await Start();
            }
            await _scheduler.PauseJob(jobKey);
        }
    }
}
