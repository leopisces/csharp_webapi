using DataService.Grpc.Contracts;
using Grpc.Core;
using Grpc.Health.V1;
using MagicOnion;
using MagicOnion.Server;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataService.GrpcService.Services
{
    /// <summary>
    /// 描述：
    /// 作者：xxx
    /// 创建日期：2022/9/5 11:03:23
    /// 版本：v1.0
    /// =============================================================
    /// 历史更新记录
    /// 版本：v1.0      
    /// 修改人：
    /// 修改日期：
    /// 修改内容：
    /// ==============================================================
    /// </summary>
    // Implements RPC service in the server project.
    // The implementation class must inherit `ServiceBase<IMyFirstService>` and `IMyFirstService`
    public class Test : ServiceBase<ITest>, ITest
    {
        /// <summary>
        /// 加法
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public async UnaryResult<string> SumAsync(int x, int y)
        {
            Console.WriteLine($"Received:{x}, {y}");
            return (x + y).ToString();
        }
    }
}