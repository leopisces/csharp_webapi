using Consul;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataService.Consul
{
    public class ServiceDiscoveryHostedService : IHostedService
    {
        private readonly IConsulClient _client;
        private readonly ServiceConfig _config;
        private string _registrationId;
        private string _guid;

        public ServiceDiscoveryHostedService(IConsulClient client, ServiceConfig config)
        {
            _client = client;
            _config = config;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _guid = Guid.NewGuid().ToString("N").Substring(0,10);
            _registrationId = $"api-{_guid}-{_config.ServiceId}";

            var registration = new AgentServiceRegistration()
            {
                ID = _registrationId,
                Name = _config.ServiceName,// 服务名
                Address = _config.ServiceAddress.Host, // 服务绑定IP(也就是你这个项目运行的ip地址)
                Port = _config.ServiceAddress.Port, // 服务绑定端口(也就是你这个项目运行的端口)
                Check = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动多久后注册
                    Interval = TimeSpan.FromSeconds(10),//健康检查时间间隔
                    HTTP = $"http://{_config.ServiceAddress.Host}:{_config.ServiceAddress.Port}/Health",//健康检查地址
                    Timeout = TimeSpan.FromSeconds(5)
                }
            };

            await _client.Agent.ServiceDeregister(registration.ID, cancellationToken);
            await _client.Agent.ServiceRegister(registration, cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _client.Agent.ServiceDeregister(_registrationId, cancellationToken);
        }
    }
}
