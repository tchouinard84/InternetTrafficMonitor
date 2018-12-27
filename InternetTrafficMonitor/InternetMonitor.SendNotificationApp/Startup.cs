using InternetMonitor.SendNotificationApp.config;
using InternetMonitor.SendNotificationApp.data;
using InternetMonitor.SendNotificationApp.sender;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace InternetMonitor.SendNotificationApp
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.production.json", optional: true, reloadOnChange: true);
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);
            services.AddOptions();

            services.Configure<AppConfig>(Configuration.GetSection("AppConfig"));
            services.Configure<EmailConfig>(Configuration.GetSection("EmailConfig"));

            services.AddScoped<IHistorySender, HistorySender>();
            services.AddScoped<IHistoryData, HistoryData>();

            services.AddScoped<App>();
        }
    }
}
