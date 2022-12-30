using DataService.Application.Contracts.BaseInfo;
using DataService.Shared.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataService.HostApi.Jobs
{
    /// <summary>
    /// 描述：定时清理ReqUrl表
    /// 作者：Leppisces
    /// 创建日期：2022/8/9 15:17:20
    /// 版本：v1.0
    /// =============================================================
    /// 历史更新记录
    /// 版本：v1.0      
    /// 修改人：
    /// 修改日期：
    /// 修改内容：
    /// ==============================================================
    /// </summary>
    public class ReqUrlCleanJob : IExcuteJobImp
    {
        private readonly ILogger<ReqUrlCleanJob> _logger;
        private readonly IBaseReqUrlImp _iBaseReqUrlImp;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ReqUrlCleanJob(IBaseReqUrlImp iBaseReqUrlImp
            , ILogger<ReqUrlCleanJob> logger)
        {
            _logger = logger;
            _iBaseReqUrlImp = iBaseReqUrlImp;
        }

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <returns></returns>
        public async Task Execute()
        {
            //清理3天以上没有调用的url
            var reqUrls = await _iBaseReqUrlImp.GetByExpAsync(it => it.UpdateDate != null && SqlFunc.DateAdd(it.UpdateDate.Value, 3) <= DateTime.Now);
            reqUrls.ForEach(item => item.Enabled = false);

            if (reqUrls.Count > 0)
            {
                var r = await _iBaseReqUrlImp.UpdateManyAsync(reqUrls);
                if (!r) _logger.LogInformation("定时清理ReqUrl表失败!");
                else _logger.LogInformation("定时清理ReqUrl表成功!");
            }

        }

        /// <summary>
        /// 执行任务
        /// </summary>
        public void ExecuteJob()
        {
            Execute().Wait();
        }
    }
}