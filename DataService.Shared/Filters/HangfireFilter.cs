using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataService.Shared.Filters
{
    /// <summary>
    /// 描述：
    /// 作者：Leopisces
    /// 创建日期：2022/8/9 15:07:42
    /// 版本：v1.0
    /// =============================================================
    /// 历史更新记录
    /// 版本：v1.0      
    /// 修改人：
    /// 修改日期：
    /// 修改内容：
    /// ==============================================================
    /// </summary>
    public class HangfireFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
       {
            var httpContext = context.GetHttpContext();

            return true;  // 允许远程无限制访问   
        }
    }
}
