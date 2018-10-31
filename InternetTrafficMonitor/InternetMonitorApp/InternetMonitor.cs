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

        public InternetMonitor(IOptions<AppConfig> configOptions, IInternetHistory history)
        {
            _config = configOptions.Value;
            _history = history;
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
                Thread.Sleep(1000);
                CheckChromeProcess();
            }
        }

        public void CheckChromeProcess()
        {
            foreach (var process in Process.GetProcessesByName("chrome"))
            {
                MaybeAddInternetHistoryEntry(process);
            }
        }

        private void MaybeAddInternetHistoryEntry(Process process)
        {
            var url = ChromeUrlRetriever.GetUrl(process);
            if (url == null) { return; }
            if (url.Equals(_currentUrl)) { return; }

            _currentUrl = url;

            var title = process.MainWindowTitle;

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
