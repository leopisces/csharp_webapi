using AutoMapper;
using DataService.HostApi.Controllers.Base;
using DataService.Mongo.IRepository;
using DataService.Mongo.Models;
using DataService.Redis;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace DataService.HostApi.Acivators
{
    /// <summary>
    /// 基类控制器中的伪属性注入
    /// </summary>
    public class XcServiceBasedControllerActivator : IControllerActivator
    {
        /// <summary>
        /// Create
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object Create(ControllerContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException(nameof(actionContext));
            }

            var controllerType = actionContext.ActionDescriptor.ControllerTypeInfo.AsType();

            //获取Controller实例
            var controller = actionContext.HttpContext.RequestServices.GetRequiredService(controllerType);

            //判断是否集成了自定义的Controller基类
            if (controller is BaseController controllerBase)
            {
                //为基类属性赋值，实现伪属性注入
                controllerBase.Logger = actionContext.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(controllerType);
                controllerBase.RedisService = actionContext.HttpContext.RequestServices.GetRequiredService<IRedisService>();
                controllerBase.Mapper = actionContext.HttpContext.RequestServices.GetRequiredService<IMapper>();
                controllerBase.Publisher = actionContext.HttpContext.RequestServices.GetRequiredService<ICapPublisher>();
                controllerBase.Configuration = actionContext.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
                controllerBase.MongoRepository = actionContext.HttpContext.RequestServices.GetRequiredService<IBaseMongoRepository>();
            }

            return controller;
        }

        /// <summary>
        /// Release
        /// </summary>
        /// <param name="context"></param>
        /// <param name="controller"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Release(ControllerContext context, object controller)
        {
        }
    }
}
