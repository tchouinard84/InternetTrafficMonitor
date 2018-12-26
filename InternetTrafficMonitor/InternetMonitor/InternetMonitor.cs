using InternetMonitor.url;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace InternetMonitor
{
    public class InternetMonitor
    {
        private const string BASE_DIR = @"C:\Users\tchouina\Personal\InternetMonitor";
        private const string ALERT_ITEMS_FILE_PATH = BASE_DIR + @"\alert_words.txt";
        private const string IGNORE_ITEMS_FILE_PATH = BASE_DIR + @"\ignore_items.txt";

        private readonly InternetHistory _history;
        private List<string> _ignoreItems;
        private string _currentUrl;
        private bool _running;

        private readonly IEnumerable<IUrlRetriever> _urlRetrievers;

        private static IEnumerable<string> AlertWords => File.ReadAllLines(ALERT_ITEMS_FILE_PATH);

        public InternetMonitor()
        {

            _history = new InternetHistory(AlertWords);
            _urlRetrievers = new List<IUrlRetriever>
            {
                new ChromeUrlRetriever(),
                new InternetExplorerUrlRetriever(),
                new FirefoxUrlRetriever()
            };
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
                CheckProcesses();
            }
        }

        public void CheckProcesses()
        {
            foreach (var urlRetriever in _urlRetrievers)
            {
                foreach (var process in Process.GetProcessesByName(urlRetriever.Browser))
                {
                    var url = urlRetriever.GetUrl(process);
                    MaybeAddInternetHistoryEntry(url, process.MainWindowTitle);
                }
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
            var ignoreItems = File.ReadAllLines(IGNORE_ITEMS_FILE_PATH);
            foreach (var item in ignoreItems) { _ignoreItems.Add(item); }
        }
    }
}
