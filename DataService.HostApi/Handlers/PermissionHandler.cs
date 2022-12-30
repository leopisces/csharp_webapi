using DataService.Application.Contracts.BaseInfo;
using DataService.Domain.BaseInfo;
using DataService.JWT;
using DataService.Shared.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DataService.HostApi.Handlers
{
    /// <summary>
    /// 权限授权Handler
    /// </summary>
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly ILogger<PermissionHandler> _logger;
        private readonly IBaseUserImp _iBaseUserImp;
        private readonly IBaseReqUrlImp _iBaseReqUrlImp;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 验证方案提供对象
        /// </summary>
        public IAuthenticationSchemeProvider Schemes { get; set; }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="schemes"></param>
        /// <param name="accessor"></param>
        /// <param name="logger"></param>
        /// <param name="iBaseUserImp"></param>
        /// <param name="iBaseReqUrlImp"></param>
        /// <param name="configuration"></param>
        public PermissionHandler(IAuthenticationSchemeProvider schemes
            , IHttpContextAccessor accessor
            , ILogger<PermissionHandler> logger
            , IBaseUserImp iBaseUserImp
            , IBaseReqUrlImp iBaseReqUrlImp
            , IConfiguration configuration
        )
        {
            Schemes = schemes;
            this._accessor = accessor;
            this._logger = logger;
            _iBaseUserImp = iBaseUserImp;
            _iBaseReqUrlImp = iBaseReqUrlImp;
            _configuration = configuration;
        }

        /// <summary>
        /// 获取用户id
        /// </summary>
        /// <returns></returns>
        public int? GetUserId()
        {
            if (_accessor.HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                string token = _accessor.HttpContext.Request.Headers["Authorization"].ToString().Substring("Bearer ".Length).Trim();

                var jwtArr = token.Split('.');

                var payLoad = JsonConvert.DeserializeObject<Dictionary<string, object>>(Base64UrlEncoder.Decode(jwtArr[1]));
                return Convert.ToInt32(payLoad["UserID"]);
            }
            return null;
        }

        /// <summary>
        /// HandleRequirementAsync
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var httpContext = _accessor.HttpContext;
            var userId = GetUserId();
            if (userId == null)
            {
                context.Fail();
                return;
            }

            //请求Url
            var requestUrl = httpContext.Request.Path.Value.ToLower();

            if (!await _iBaseReqUrlImp.Exist<Base_ReqUrl>(it => it.Url == requestUrl))
            {
                var r = await _iBaseReqUrlImp.AddAsync(new Base_ReqUrl
                {
                    Url = requestUrl,
                    CreateDate = DateTime.Now,
                    CreaterId = userId,
                    Count = 1,
                    Enabled = true,
                });
            }
            else
            {
                var reqUrl = await _iBaseReqUrlImp.GetByExpAsync(it => it.Url == requestUrl);
                reqUrl.ForEach(item =>
                {
                    item.UpdateId = userId;
                    item.UpdateDate = DateTime.Now;
                    item.Count++;
                    item.Enabled = true;
                });
                var r = await _iBaseReqUrlImp.UpdateManyAsync(reqUrl);
            }

            //判断请求是否停止
            var handlers = httpContext.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
            foreach (var scheme in await Schemes.GetRequestHandlerSchemesAsync())
            {
                var handler = await handlers.GetHandlerAsync(httpContext, scheme.Name) as IAuthenticationRequestHandler;
                if (handler != null && await handler.HandleRequestAsync())
                {
                    context.Fail();
                    return;
                }
            }
            //判断请求是否拥有凭据，即有没有登录
            var defaultAuthenticate = await Schemes.GetDefaultAuthenticateSchemeAsync();
            if (defaultAuthenticate != null)
            {
                var result = await httpContext.AuthenticateAsync(defaultAuthenticate.Name);
                //result?.Principal不为空即登录成功
                if (result?.Principal != null)
                {
                    var projectConfig = _configuration.GetSection("Project").Get<ProjectConfig>();
                    // 判断接口访问权限
                    if (projectConfig.UrlAuth)
                    {
                        await UrlAuth(context, requirement, result, requestUrl, userId);
                    }

                    //判断过期时间
                    if (DateTime.Parse(httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Expiration).Value) >= DateTime.Now)
                    {
                        context.Succeed(requirement);
                    }
                    else
                    {
                        context.Fail();
                    }
                    return;
                }
            }
            //判断没有登录时，是否访问登录的url,并且是Post请求，并且是form表单提交类型，否则为失败
            if (!requestUrl.Equals(requirement.LoginPath.ToLower(), StringComparison.Ordinal) && (!httpContext.Request.Method.Equals("POST")
               || !httpContext.Request.HasFormContentType))
            {
                context.Fail();
                return;
            }
            context.Succeed(requirement);
        }

        /// <summary>
        /// Url权限验证
        /// </summary>
        /// <returns></returns>
        private async Task UrlAuth(AuthorizationHandlerContext context, PermissionRequirement requirement, AuthenticateResult result, string requestUrl, int? userId)
        {
            var httpContext = _accessor.HttpContext;
            var dic = GetUserId();
            //获取当前用户下的角色路径记录
            var userInfo = await _iBaseUserImp.GetByExpIncludAsync(it => it.UserId == userId, it => it.ClaimRole, role => role.RoleUrl, url => url.ReqUrl);
            var roleName = "";
            if (userInfo != null && userInfo.Count == 1)
            {
                roleName = userInfo.First().ClaimRole.Name;
            }
            else
            {
                context.Fail();
                return;
            }
            var _permissions = userInfo.First().ClaimRole.RoleUrl.Select(it => new
            {
                Name = roleName,
                Url = it.ReqUrl.Url
            }).ToList();
            //无接口访问权限
            if (!string.IsNullOrEmpty(roleName) && _permissions.Count == 0)
            {
                context.Fail();
                return;
            }
            _accessor.HttpContext.User = result.Principal;
            //权限中是否存在请求的url
            if (_permissions != null && _permissions.GroupBy(g => g.Url).Where(w => w.Key.ToLower() == requestUrl).Count() > 0)
            {
                var name = httpContext.User.Claims.SingleOrDefault(s => s.Type == requirement.ClaimType).Value;
                //验证权限
                if (_permissions.Where(w => w.Name == name && w.Url.ToLower() == requestUrl).Count() == 0)
                {
                    //无权限跳转到拒绝页面   
                    //httpContext.Response.Redirect(requirement.DeniedAction);
                    context.Fail();
                    return;
                }
            }
            else
            {
                context.Fail();
                return;

            }
        }
    }
}