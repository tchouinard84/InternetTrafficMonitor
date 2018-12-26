using InternetMonitorApp.config;
using InternetMonitorApp.url;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace InternetMonitorApp
{
    public class InternetMonitor : IInternetMonitor
    {
        private readonly AppConfig _config;
        private readonly IInternetHistory _history;
        private List<string> _ignoreItems;
        private string _currentUrl;
        private bool _running;

        private readonly IUrlRetriever _chromeUrlRetriever;
        private readonly IUrlRetriever _ieUrlRetriever;
        private readonly IUrlRetriever _firefoxUrlRetriever;

        public InternetMonitor(IOptions<AppConfig> configOptions, IInternetHistory history,
            IUrlRetriever chromeUrlRetriever, IUrlRetriever ieUrlRetriever, IUrlRetriever firefoxUrlRetriever)
        {
            _config = configOptions.Value;
            _history = history;
            _chromeUrlRetriever = chromeUrlRetriever;
            _ieUrlRetriever = ieUrlRetriever;
            _firefoxUrlRetriever = firefoxUrlRetriever;
            _currentUrl = "";

            InitializeIgnoreItems();
        }

        public void Start(string comment)
        {
            _history.Start(comment);
            _running = true;
            Run();
        }

        public void Stop(string reason)
        {
            _history.Stop(reason);
            _running = false;
        }

        private void Run()
        {
            while (_running)
            {
                Thread.Sleep(750);
                CheckChromeProcesses();
                CheckInternetExplorerProcesses();
                CheckFirefoxProcesses();
            }
        }

        public void CheckChromeProcesses()
        {
            foreach (var process in Process.GetProcessesByName("chrome"))
            {
                var url = _chromeUrlRetriever.GetUrl(process);
                MaybeAddInternetHistoryEntry(url, process.MainWindowTitle);
            }
        }

        public void CheckInternetExplorerProcesses()
        {
            foreach (var process in Process.GetProcessesByName("iexplore"))
            {
                var url = _ieUrlRetriever.GetUrl(process);
                MaybeAddInternetHistoryEntry(url, process.MainWindowTitle);
            }
        }

        public void CheckFirefoxProcesses()
        {
            foreach (var process in Process.GetProcessesByName("firefox"))
            {
                var url = _firefoxUrlRetriever.GetUrl(process);
                MaybeAddInternetHistoryEntry(url, process.MainWindowTitle);
            }
        }

        private void MaybeAddInternetHistoryEntry(string url, string title)
        {
            if (url == null) { return; }
            if (url.Equals(_currentUrl)) { return; }

            _currentUrl = url;

            if (title == string.Empty) { return; }

            foreach (var item in _ignoreItems)
            {
                if (title.Contains(item)) { return; }
            }

            _history.MaybeAddHistory(title, url);
        }

        private void InitializeIgnoreItems()
        {
            _ignoreItems = new List<string>();
            var ignoreItems = File.ReadAllLines(_config.GetIgnoreItemsFilePath());
            foreach (var item in ignoreItems) { _ignoreItems.Add(item); }
        }
    }
}
