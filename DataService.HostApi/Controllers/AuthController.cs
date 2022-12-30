using DataService.Shared.Base;
using DataService.JWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using DataService.HostApi.Controllers.Base;
using System.Collections.Generic;
using DataService.Shared.Helpers;
using DataService.Shared.Exceptions;
using System.Linq;
using DataService.Redis;
using DataService.Redis.Config;
using DataService.Swagger;
using Microsoft.Extensions.Logging;
using DataService.Application.Contracts.BaseInfo;
using DataService.Domain.BaseInfo;
using AutoMapper;
using DataService.Dto.BaseInfo;

namespace DataService.HostApi.Controllers
{
    /// <summary>
    /// 身份验证
    /// </summary>
    [ApiGroup(GroupVersion.WEBAPI)]
    public class AuthController : BaseController
    {
        /// <summary>
        /// 自定义策略参数
        /// </summary>
        private readonly PermissionRequirement _requirement;
        private readonly IBaseUserImp _iBaseUserImp;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iBaseUserImp"></param>
        /// <param name="requirement"></param>
        public AuthController(IBaseUserImp iBaseUserImp
            , PermissionRequirement requirement
        )
        {
            _requirement = requirement;
            _iBaseUserImp = iBaseUserImp;
        }

        /// <summary>
        /// 无权访问
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public void Denied()
        {
            throw new AuthException("没有接口访问权限!");
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]    //不启用token验证
        public async Task<LoginInfoDto> Login([Required] string account, [Required] string password)
        {
            // 账号验证
            var user = new Base_User();
            var pwdStr = AESCryptoHelper.AESEncrypt(password);
            var users = await _iBaseUserImp.GetByExpIncludAsync(it => it.Account == account && it.State == 1, it => it.ClaimRole);
            if (users.Count == 1)
            {
                user = users.FirstOrDefault();
                if (user.Password != pwdStr)
                {
                    throw new LoginException(MessageHelper.ACCOUNT_OR_PASSWORD_ERROR);
                }
            }
            else
            {
                throw new LoginException(MessageHelper.ACCOUNT_NOT_EXISTS);
            }

            // 如果是基于用户的授权策略，这里要添加用户;如果是基于角色的授权策略，这里要添加角色
            IPAddress ip = HttpContext.Connection.LocalIpAddress;
            if (ip == null)
            {
                ip = IPAddress.Parse("127.0.0.1");
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.ClaimRole.Description),
                new Claim(ClaimTypes.Role, user.ClaimRole.Name),
                // 日期格式更改
                new Claim(ClaimTypes.Expiration,  DateTime.Now.AddSeconds(_requirement.Expiration.TotalSeconds).ToString("yyyy-MM-dd HH:mm:ss")),
                new Claim("UserID",user.UserId.ToString()) ,
                new Claim("PlatForm", "Web") ,
                new Claim("IP", ip.MapToIPv4().ToString())
            };
            // 用户标识
            var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
            identity.AddClaims(claims);

            var token = await JwtToken.BuildJwtToken(claims, _requirement);
            LoginInfo login = new LoginInfo
            {
                Status = token.Status,
                AccessToken = token.Access_Token,
                ExpiresIn = token.Expires_In / 1000,
                TokenType = token.Token_Type,
            };
            if (password == "1234") login.ResetPwd = true;
            // 获取登录用户信息
            login.BasicInfo = new ClientInformation
            {
                ID = user.UserId,
                Name = user.Name,
                Phone = user.Phone,
                Account = user.Account
            };
            // 存入Redis缓存
            var s = await RedisService.StringSetAsync(string.Format(RedisConfigure.USERINFOS.RedisKey, user.UserId)
                , login
                , TimeSpan.FromSeconds(login.ExpiresIn)
                , RedisConfigure.USERINFOS.RedisIndex
            );
            if (!s) throw new LoginException(MessageHelper.REDIS_CACHE_FAILD);
            return Mapper.Map<LoginInfo, LoginInfoDto>(login);
        }
    }
}
