using Microsoft.Extensions.DependencyInjection;
using System;

namespace InternetMonitor.SendNotificationApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();
            var startup = new Startup();
            startup.ConfigureServices(services);
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetService<App>().Run();
        }
    }
}
