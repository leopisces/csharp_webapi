
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataService.JWT
{
    /// <summary>
    /// JwtBearer扩展
    /// </summary>
    public static class JwtBearerExtension
    {
        public static IServiceCollection AddJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var audienceConfig = configuration.GetSection("Audience").Get<AudienceConfig>();
            //应用API的Token
            if (audienceConfig.Identification == "Producer")
            {
                services.AddJTokenBuild(audienceConfig.ValidateHour, audienceConfig.Issuer, audienceConfig.Issuer, audienceConfig.Secret, "/api/auth/denied");
            }
            else if (audienceConfig.Identification == "Use")
            {
                services.AddOcelotPolicyJwtBearer(audienceConfig.ValidateHour, audienceConfig.Issuer, audienceConfig.Issuer, audienceConfig.Secret, "Bearer", "Permission", "/api/auth/denied");
            }
            return services;
        }

        /// <summary>
        /// 注入Ocelot下JwtBearer，在ocelot网关的Startup的ConfigureServices中调用
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <param name="issuer">发行人</param>
        /// <param name="audience">订阅人</param>
        /// <param name="secret">密钥</param>
        /// <param name="defaultScheme">默认架构</param>
        /// <param name="isHttps">是否https</param>
        /// <returns></returns>
        public static AuthenticationBuilder AddOcelotJwtBearer(this IServiceCollection services, string issuer, string audience, string secret, string defaultScheme, bool isHttps = false)
        {

            var keyByteArray = Encoding.ASCII.GetBytes(secret);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = issuer,//发行人
                ValidateAudience = true,
                ValidAudience = audience,//订阅人
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,
            };
            return services.AddAuthentication(options =>
            {
                options.DefaultScheme = defaultScheme;
            })
             .AddJwtBearer(defaultScheme, opt =>
             {
                 //不使用https
                 opt.RequireHttpsMetadata = isHttps;
                 opt.TokenValidationParameters = tokenValidationParameters;
             });
        }

        /// <summary>
        /// 注入Ocelot jwt策略，在业务API应用中的Startup的ConfigureServices调用
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <param name="expirationhour">过期时间,按小时</param>
        /// <param name="issuer">发行人</param>
        /// <param name="audience">订阅人</param>
        /// <param name="secret">密钥</param>
        /// <param name="defaultScheme">默认架构</param>
        /// <param name="policyName">自定义策略名称</param>
        /// <param name="deniedUrl">拒绝路由</param>
        /// <param name="isHttps">是否https</param>
        /// <returns></returns>
        public static AuthenticationBuilder AddOcelotPolicyJwtBearer(this IServiceCollection services, int expirationhour, string issuer, string audience, string secret, string defaultScheme, string policyName, string deniedUrl, bool isHttps = false)
        {   //添加
            var keyByteArray = Encoding.ASCII.GetBytes(secret);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = issuer,//发行人
                ValidateAudience = true,
                ValidAudience = audience,//订阅人
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,

            };
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            // 如果第三个参数，是ClaimTypes.Role，上面集合的每个元素的Name为角色名称，如果ClaimTypes.Name，即上面集合的每个元素的Name为用户名
            var permissionRequirement = new PermissionRequirement(
                deniedUrl,
                ClaimTypes.Role,
                issuer,
                audience,
                signingCredentials,
                expiration: TimeSpan.FromHours(expirationhour)
            );
            services.AddSingleton(permissionRequirement);
            return services.AddAuthorization(options =>     // 导入角色身份认证策略
                           {
                               options.AddPolicy(policyName, policy => policy.Requirements.Add(permissionRequirement));
                           })
                           .AddAuthentication(options =>    // 身份认证类型
                           {
                               options.DefaultScheme = defaultScheme;
                           })
                           .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
                           {
                               // 不使用https
                               o.RequireHttpsMetadata = isHttps;
                               o.TokenValidationParameters = tokenValidationParameters;
                               o.Events = new JwtBearerEvents
                               {
                                   // 请求权限表鉴权失败
                                   OnForbidden = context =>
                                   {
                                       context.Response.Redirect(permissionRequirement.DeniedAction);
                                       return Task.CompletedTask;
                                   }
                               };
                           });
        }
        /// <summary>
        /// 注入Token生成器参数，在token生成项目的Startup的ConfigureServices中使用
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <param name="expirationhour">过期时间,按小时</param>
        /// <param name="issuer">发行人</param>
        /// <param name="audience">订阅人</param>
        /// <param name="secret">密钥</param>
        /// <param name="deniedUrl">拒绝路由</param>
        /// <returns></returns>
        public static IServiceCollection AddJTokenBuild(this IServiceCollection services, int expirationhour, string issuer, string audience, string secret, string deniedUrl)
        {
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)), SecurityAlgorithms.HmacSha256);
            // 如果第三个参数，是ClaimTypes.Role，上面集合的每个元素的Name为角色名称，如果ClaimTypes.Name，即上面集合的每个元素的Name为用户名
            var permissionRequirement = new PermissionRequirement(
                deniedUrl
                , ClaimTypes.Role
                , issuer
                , audience
                , signingCredentials
                , expiration: TimeSpan.FromHours(expirationhour)
            );
            return services.AddSingleton(permissionRequirement);
        }
    }
}
