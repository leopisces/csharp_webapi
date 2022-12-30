using DataService.HostApi.Controllers.Base;
using DataService.Swagger;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DataService.HostApi.Controllers.Test
{
    /// <summary>
    /// Cap分布式事务测试
    /// </summary>
    [ApiGroup(GroupVersion.ApiTest)]
    [AllowAnonymous]
    public class CapPublisherController : BaseController
    {
        /// <summary>
        /// CAP发布消息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> TestCapPublisher()
        {
            await Publisher.PublishAsync("Order.Created", "hello，订单创建成功啦");  //发布Order.Created事件
            return "订单创建成功啦";
        }

        /// <summary>
        /// 监听事件  
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        [NonAction]
        [CapSubscribe("Order.Created", Group = "Order")] //监听Order.Created事件  
        public Task OrderCreatedEventHand(string msg)
        {
            Logger.LogInformation(msg);
            return Task.CompletedTask;
        }
    }
}
