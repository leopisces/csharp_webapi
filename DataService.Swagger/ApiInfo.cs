using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace DataService.Swagger
{
    public class ApiInfo : IApiInfo
    {
        private ApiInfo(IConfiguration config, Assembly assembly)
        {
            var projectinfo = config.GetSection("ApiInfo");
            Title = projectinfo["Title"];
            Version = projectinfo["Version"];
            ApiName = projectinfo["ApiName"];
            Description = projectinfo["Description"];
            ApplicationAssembly = assembly;
        }

        public string Title { get; set; }
        public string ApiName { get; set; }
        public string BindAddress { get; set; }
        public string Version { get; set; }
        public int BindPort { get; set; }
        public Assembly ApplicationAssembly { get; set; }
        public static IApiInfo Instance { get; private set; }

        public string Description { get; set; }

        public static IApiInfo Instantiate(IConfiguration config, Assembly assembly)
        {
            Instance = new ApiInfo(config, assembly);
            return Instance;
        }
    }
}
