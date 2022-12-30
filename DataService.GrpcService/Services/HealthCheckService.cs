using Grpc.Core;
using Grpc.Health.V1;
using MagicOnion.Server;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataService.GrpcService.Services
{
    /// <summary>
    /// 健康检查
    /// </summary>
    public class HealthCheckService : Health.HealthBase
    {
        public override Task<HealthCheckResponse> Check(HealthCheckRequest request, ServerCallContext context)
        {
            Console.WriteLine($"This is DataService.GrpcService {nameof(HealthCheckService)} Check ");
            //TODO:检查逻辑
            return Task.FromResult(new HealthCheckResponse() { Status = HealthCheckResponse.Types.ServingStatus.Serving });
        }

        public override async Task Watch(HealthCheckRequest request, IServerStreamWriter<HealthCheckResponse> responseStream, ServerCallContext context)
        {
            //TODO:检查逻辑
            await responseStream.WriteAsync(new HealthCheckResponse()
            { Status = HealthCheckResponse.Types.ServingStatus.Serving });
        }
    }
}