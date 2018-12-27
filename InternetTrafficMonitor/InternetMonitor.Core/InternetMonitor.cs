using InternetMonitor.Core.browser;
using InternetMonitor.Core.config;
using Microsoft.Extensions.Options;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace InternetMonitor.Core
{
    public class InternetMonitor : IInternetMonitor
    {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        private readonly IAppConfig _config;
        private readonly IInternetHistory _history;

        private readonly IEnumerable<IUrlRetriever> _urlRetrievers;

        public InternetMonitor()
        {
            _config = new AppConfig();
            _history = new InternetHistory();
            _urlRetrievers = new List<IUrlRetriever>
            {
                new ChromeUrlRetriever(),
                new InternetExplorerUrlRetriever(),
                new FirefoxUrlRetriever()
            };
        }

        public InternetMonitor(IOptions<AppConfig> configOptions, IInternetHistory history)
        {
            _config = configOptions.Value;
            _history = history;
            _urlRetrievers = new List<IUrlRetriever>
            {
                new ChromeUrlRetriever(),
                new InternetExplorerUrlRetriever(),
                new FirefoxUrlRetriever()
            };
        }

        public void CheckProcesses()
        {
            try
            {
                foreach (var urlRetriever in _urlRetrievers)
                {
                    foreach (var process in Process.GetProcessesByName(urlRetriever.Browser))
                    {
                        var url = urlRetriever.GetUrl(process);
                        MaybeWriteEntry(url, process.MainWindowTitle);
                    }
                }
            }
            catch (Exception e)
            {
                log.Error(e, "Error Checking Processes");
            }
        }

        private void MaybeWriteEntry(string url, string title)
        {
            try
            {
                if (url == null) { return; }
                if (title == string.Empty) { return; }
                if (GetIgnoreItems().Any(title.Contains)) { return; }
                if (_history.Contains(url, title)) { return; }

                _history.WriteEntry(title, url);
            }
            catch (Exception e)
            {
                log.Error(e, "Error with MaybeWriteEntry");
            }
        }

        private IEnumerable<string> GetIgnoreItems()
        {
            try
            {
                return File.ReadAllLines(_config.GetIgnoreItemsFilePath());
            }
            catch (Exception e)
            {
                log.Error(e, "Error trying to Get the Ignore Items");
                return new List<string>();
            }
        }
    }
}
