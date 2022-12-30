using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataService.Shared.Base
{
    public static class DIContainer
    {
        public static class ServiceLocator
        {
            public static IServiceCollection Collection { get; set; }
            public static IServiceProvider Instance { get; set; }

        }

    }
}
