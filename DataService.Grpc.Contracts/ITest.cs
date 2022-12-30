using Grpc.Core;
using Grpc.Health.V1;
using MagicOnion;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Grpc.Contracts
{
    /// <summary>
    /// 定义接口和方法，IService，UnaryResult是MagicOnion自带
    /// </summary>
    public interface ITest : IService<ITest>
    {
        UnaryResult<string> SumAsync(int x, int y);
    }
}
