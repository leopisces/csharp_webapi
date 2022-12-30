using DataService.HostApi.Controllers.Base;
using DataService.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DataService.HostApi.Controllers.Test
{
    /// <summary>
    /// Nlog测试
    /// </summary>
    [ApiGroup(GroupVersion.ApiTest)]
    [AllowAnonymous]
    public class NlogController : BaseController
    {
        /// <summary>
        /// 测试Nlog
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public int TestNlog()
        {
            Logger.LogTrace("trace");
            Logger.LogDebug("debug");
            Logger.LogInformation("info");
            Logger.LogWarning("warn");
            Logger.LogError("error");
            Logger.LogCritical("critical");
            using (Logger.BeginScope("测试日志的Scope"))
            {
                Logger.LogTrace("scope-trace");
                Logger.LogDebug("scope-debug");
                Logger.LogInformation("scope-info");
                Logger.LogWarning("scope-warn");
                Logger.LogError("scope-error");
                Logger.LogCritical("scope-critical");
            }
            Logger.LogTrace("Scope已经结束了。。。");
            return 1;
        }
    }
}
