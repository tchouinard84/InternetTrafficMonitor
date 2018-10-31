using InternetMonitorApp.config;
using InternetMonitorApp.data;
using InternetMonitorApp.sender;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using System.IO;

namespace InternetMonitorApp
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

            services.AddSingleton(LogManager.LoadConfiguration("NLog.config"));
            services.AddLogging();

            services.AddScoped<IInternetHistorySender, InternetHistorySender>();
            services.AddScoped<IInternetHistoryData, InternetHistoryData>();
            services.AddScoped<IInternetHistory, InternetHistory>();
            services.AddScoped<IInternetMonitor, InternetMonitor>();

            services.AddScoped<App>();
        }

        public void Configure(ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();
        }
    }
}
