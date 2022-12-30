using DataService.HostApi.Jobs;
using DataService.Shared.Base;
using DataService.Shared.Managers;
using DataService.Shared.Models;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TimeZoneConverter;

namespace DataService.HostApi.TaskServices
{
    /// <summary>
    /// 描述：
    /// 作者：xxx
    /// 创建日期：2022/8/9 10:59:31
    /// 版本：v1.0
    /// =============================================================
    /// 历史更新记录
    /// 版本：v1.0      
    /// 修改人：
    /// 修改日期：
    /// 修改内容：
    /// ==============================================================
    /// </summary>
    public class TestBackGroudService : BackgroundService
    {
        private readonly ILogger<TestBackGroudService> _logger;
        private readonly TimingManager _manager;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="manager"></param>
        /// <param name="configuration"></param>
        public TestBackGroudService(ILogger<TestBackGroudService> logger, TimingManager manager, IConfiguration configuration)
        {
            _logger = logger;
            _manager = manager;
            _configuration = configuration;
        }

        /// <summary>
        /// 测试方法
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Service starting");
            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    _logger.LogInformation(DateTime.Now.ToLongTimeString() + ": Refresh Token!");//在此写需要执行的任务
            //    await Task.Delay(5000, stoppingToken);
            //}

            //Quartz定时任务
            await _manager.Start<QuartzTestJob>();

            //Hangfire定时任务
            var jobs = _configuration.GetSection("JobConfigs").Get<List<BackJobs>>();
            if (jobs != null && jobs.Count > 0)
            {
                foreach (var job in jobs)
                {
                    if (job.Enabled) //开启任务
                    {
                        switch (job.JobName)
                        {
                            case "Api请求路径表定时清理":
                                RecurringJob.AddOrUpdate<ReqUrlCleanJob>(job.JobName
                                    , x => x.ExecuteJob()
                                    , job.Interval
                                    //, Cron.Minutely   //注意最小单位是分钟
                                    , TZConvert.GetTimeZoneInfo("Asia/Shanghai"));
                                break;
                        }
                    }
                    else
                    {
                        RecurringJob.RemoveIfExists(job.JobName);
                    }
                }
            }

            _logger.LogInformation("Service stopping");
        }
    }
}