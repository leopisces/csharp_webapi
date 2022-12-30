using Consul;
using DataService.Swagger;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net.Http;

namespace DataService.HostApi.Controllers.Test
{
    /// <summary>
    /// Grpc服务测试
    /// </summary>
    /// <returns></returns>
    [ApiGroup(GroupVersion.ApiTest)]
    [AllowAnonymous]
    public class GrpcServiceController : Controller
    {
    }
}
