using InternetMonitor.Framework.Core.Browser;
using InternetMonitor.Framework.Core.Config;
using InternetMonitor.Framework.Core.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace InternetMonitor.Framework.Core
{
    public class InternetMonitor : IInternetMonitor
    {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        private readonly AppConfig _config;
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

                var type = DetermineType(url, title);
                _history.WriteEntry(type, title, url);
            }
            catch (Exception e)
            {
                log.Error(e, "Error with MaybeWriteEntry");
            }
        }

        private LogType DetermineType(string title, string url)
        {
            foreach (var alertWord in GetAlertItems())
            {
                if (title.ToLower().Contains(alertWord)) { return LogType.Alert; }
                if (url.ToLower().Contains(alertWord)) { return LogType.Alert; }
            }
            return LogType.Info;
        }

        private IEnumerable<string> GetAlertItems() => File.ReadAllLines(_config.GetAlertItemsFilePath());

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
