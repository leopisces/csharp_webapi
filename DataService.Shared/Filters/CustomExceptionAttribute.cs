using DataService.Shared.Base;
using DataService.Domain.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using DataService.Shared.Exceptions;

namespace DataService.Domain.Shared.Filters
{
    /// <summary>
    /// 统一异常处理
    /// </summary>
    public class CustomExceptionAttribute : IExceptionFilter
    {
        // 日志
        private readonly ILogger<CustomExceptionAttribute> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        public CustomExceptionAttribute(
            ILogger<CustomExceptionAttribute> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 发生异常处理
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            context.ExceptionHandled = true;

            // 参数异常
            if (context.Exception is ParamsException)
            {
                context.Result = new OkObjectResult(new ResponseResult(ErrorCodes.PARAMS_ERROR, context.Exception.Message));
                return;
            }

            // 登录异常
            if (context.Exception is LoginException)
            {
                context.Result = new OkObjectResult(new ResponseResult(ErrorCodes.LOGIN_ERROR, context.Exception.Message));
                return;
            }

            // token异常
            if (context.Exception is TokenException)
            {
                context.Result = new OkObjectResult(new ResponseResult(ErrorCodes.TOKEN_ERROR, context.Exception.Message));
                return;
            }

            // 记录错误日志
            _logger.LogError(new EventId(context.Exception.HResult), context.Exception, context.Exception.Message);

            context.Result = new OkObjectResult(new ResponseResult(ErrorCodes.SERVE_ERROR, context.Exception.Message));
        }

    }
}
