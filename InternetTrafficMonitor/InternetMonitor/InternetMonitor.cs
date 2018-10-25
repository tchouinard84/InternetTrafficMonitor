using InternetMonitor.models;
using InternetMonitor.url;
using System.Collections.Generic;
using System.Configuration;
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
        private readonly InternetLog _internetLog;
        private List<string> _ignoreItems;
        private string _currentTitle;
        private string _currentUrl;
        private bool _running;

        private static IEnumerable<string> AlertWords => File.ReadAllLines(ALERT_ITEMS_FILE_PATH);

        public InternetMonitor()
        {

            _history = new InternetHistory(AlertWords);
            _internetLog = new InternetLog();
            _currentTitle = "";
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
            _internetLog.Log(LogType.Stop, $"Stopping: {reason}");
        }

        private void Run()
        {
            _internetLog.Log(LogType.Start, "Start Log.");

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
                MaybeAddAndLogTab(process);
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

            _currentTitle = title;

            foreach (var item in _ignoreItems)
            {
                if (title.Contains(item)) { return; }
            }

            _history.MaybeAddHistory(title, url);
        }

        private void MaybeAddAndLogTab(Process process)
        {
            if (bool.Parse(ConfigurationManager.AppSettings["loggingOff"])) { return; }

            var tab = process.MainWindowTitle;

            if (tab == string.Empty) { return; }
            if (tab == _currentTitle) { return; }

            _currentTitle = tab;
            _internetLog.MaybeAddAndLog(_currentTitle);
        }

        private void InitializeIgnoreItems()
        {
            _ignoreItems = new List<string>();
            var ignoreItems = File.ReadAllLines(IGNORE_ITEMS_FILE_PATH);
            foreach (var item in ignoreItems) { _ignoreItems.Add(item); }
        }
    }
}
