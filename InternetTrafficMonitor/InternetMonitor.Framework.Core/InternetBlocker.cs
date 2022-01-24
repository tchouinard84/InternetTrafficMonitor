using InternetMonitor.Framework.Core.browser;
using InternetMonitor.Framework.Core.config;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace InternetMonitor.Framework.Core
{
    public class InternetBlocker : IInternetMonitor
    {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        private readonly AppConfig _config;
        private readonly IInternetHistory _history;

        private readonly IEnumerable<IUrlRetriever> _urlRetrievers;

        public InternetBlocker()
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
                        var block = MaybeBlock(url, process.MainWindowTitle);
                        if (block)
                        {
                            log.Info($"Blocking {process.MainWindowTitle} : {url}");
                            //process.Kill();
                            //process.Close();
                            process.CloseMainWindow();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                log.Error(e, "Error Checking Processes");
            }
        }

        private bool MaybeBlock(string url, string title)
        {
            try
            {
                if (url == null) { return false; }
                if (title == string.Empty) { return false; }
                if (GetIgnoreItems().Any(title.Contains)) { return false; }

                return MaybeBlockItem(url, title);
            }
            catch (Exception e)
            {
                log.Error(e, "Error with MaybeWriteEntry");
                return false;
            }
        }

        private bool MaybeBlockItem(string url, string title)
        {
            foreach (var blockWords in GetBlockItems())
            {
                if (title.ToLower().Contains(blockWords)) { return true; }
                if (url.ToLower().Contains(blockWords)) { return true; }
            }
            return false;
        }

        private IEnumerable<string> GetIgnoreItems() => GetConfigItems(_config.IgnoreItems);
        private IEnumerable<string> GetBlockItems() => GetConfigItems(_config.BlockItems);

        private static IEnumerable<string> GetConfigItems(string items)
        {
            if (string.IsNullOrEmpty(items)) { return Enumerable.Empty<string>(); }

            try
            {
                return items.Split(',').Select(x => x.Trim());
            }
            catch (Exception e)
            {
                log.Error(e, "Error trying to Get the Ignore Items");
                return Enumerable.Empty<string>();
            }
        }
    }
}
