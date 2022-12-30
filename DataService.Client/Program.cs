using Consul;
using DataService.Grpc.Contracts;
using Grpc.Net.Client;
using MagicOnion.Client;
using System;
using System.Threading;

namespace DataService.Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Thread.Sleep(10000);

            var serviceName = "DataService.GrpcService";
            var consulClient = new ConsulClient(c => c.Address = new Uri("http://consul.leopisces.cn"));
            var services =  consulClient.Catalog.Service(serviceName).Result;
            if (services.Response.Length == 0)
            {
                throw new Exception($"未发现服务 {serviceName}");
            }

            var service = services.Response[0];
            var address = $"http://{service.ServiceAddress}:{service.ServicePort}";

            Console.WriteLine($"获取服务地址成功：{address}");

            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            // Connect to the server using gRPC channel.
            var channel = GrpcChannel.ForAddress(address);

            // NOTE: If your project targets non-.NET Standard 2.1, use `Grpc.Core.Channel` class instead.
            // var channel = new Channel("localhost", 5001, new SslCredentials());

            // Create a proxy to call the server transparently.
            var client = MagicOnionClient.Create<ITest>(channel);

            // Call the server-side method using the proxy.
            var result = client.SumAsync(123, 456).ResponseAsync.Result;
            Console.WriteLine($"Result: {result}");
            Console.ReadKey();
        }
    }
}
