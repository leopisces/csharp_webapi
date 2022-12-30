using AutoMapper;
using DataService.Mongo.IRepository;
using DataService.Mongo.Models;
using DataService.Redis;
using DataService.Redis.Config;
using DataService.Shared.Base;
using DataService.Shared.Exceptions;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataService.HostApi.Controllers.Base
{
    /// <summary>
    /// 基类控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize("Permission")]
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// 公共配置
        /// </summary>
        public IConfiguration Configuration;
        /// <summary>
        /// 事件发布者
        /// </summary>
        public ICapPublisher Publisher;
        /// <summary>
        /// 实体自动映射
        /// </summary>
        public IMapper Mapper;
        /// <summary>
        /// 日志对象
        /// </summary>
        public ILogger Logger;
        /// <summary>
        /// Redis服务
        /// </summary>
        public IRedisService RedisService;
        /// <summary>
        /// Mongodb服务
        /// </summary>
        public IBaseMongoRepository MongoRepository;
        /// <summary>
        /// 用户信息
        /// </summary>
        protected ClientInformation client { get => GetClientInformation(); }

        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseController()
        {
            if (HttpContext != null)
                /*允许BODY里的信息多次读取*/
                HttpContext.Request.EnableBuffering();
        }

        /// <summary>
        /// 从TOKEN获取人员信息
        /// </summary>
        /// <returns></returns>
        protected ClientInformation GetClientInformation()
        {
            var _token = string.Empty;

            if (HttpContext.Request.Headers.ContainsKey("Authorization"))
                _token = HttpContext.Request.Headers["Authorization"].ToString().Substring("Bearer ".Length).Trim();
            else
                throw new TokenException("token不存在");

            var jwtArr = _token.Split('.');
            var payLoad = JsonConvert.DeserializeObject<Dictionary<string, object>>(Base64UrlEncoder.Decode(jwtArr[1]));
            var key = string.Format(RedisConfigure.USERINFOS.RedisKey, payLoad["UserID"].ToString());

            if (!string.IsNullOrEmpty(key))
            {
                if (RedisService.KeyExists(key) == false)//判断当前key的缓存对象是否存在
                {
                    //指定状态码抛出异常
                    throw new TokenException("token失效");
                }
                else
                {
                    RedisService.KeyExpire(key, TimeSpan.FromHours(2));
                    //从缓存获取数据
                    return RedisService.StringGet<LoginInfo>(key).BasicInfo;
                }
            }
            else
            {
                throw new TokenException("token解析失败");
            }
        }
    }
}
