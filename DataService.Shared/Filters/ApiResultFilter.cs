using DataService.Shared.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Linq;

namespace DataService.Domain.Shared.Filters
{
    /// <summary>
    /// 统一返回格式
    /// </summary>
    public class ApiResultFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// OnActionExecuting
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }

        /// <summary>
        /// OnResultExecuting
        /// </summary>
        /// <param name="context"></param>
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var result = context.Result as ObjectResult;

            if (result == null)
            {
                return;
            }
            if (context.Result is BadRequestObjectResult)
            {
                var data = context.ModelState.Keys
                                  .SelectMany(key => context.ModelState[key].Errors.Select(x => new BaseValidationError(key, x.ErrorMessage)))
                                  .ToList();
                context.Result = new OkObjectResult(new ResponseResult(ErrorCodes.PARAMS_ERROR, data));
            }
            else
            {
                context.Result = new OkObjectResult(new ResponseResult(ErrorCodes.SUCCESS, data: result.Value));
            }
        }
    }
}
