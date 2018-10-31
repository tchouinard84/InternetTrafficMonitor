using InternetMonitorApp.config;
using InternetMonitorApp.data;
using InternetMonitorApp.models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace InternetMonitorApp
{
    public class InternetHistory : IInternetHistory
    {
        private readonly AppConfig _config;
        private readonly IInternetHistoryData _data;
        private List<string> _alertItems;

        public Dictionary<string, List<InternetHistoryEntry>> History { get; private set; }

        public InternetHistory(IOptions<AppConfig> configOptions, IInternetHistoryData data)
        {
            _config = configOptions.Value;
            _data = data;
            History = new Dictionary<string, List<InternetHistoryEntry>>();

            InitializeAlertItems();
        }

        public bool Contains(string title) => History.ContainsKey(title);

        public void MaybeAddHistory(string title, string url)
        {
            if (!History.ContainsKey(title))
            {
                History.Add(title, new List<InternetHistoryEntry> { BuildAndWriteInternetHistoryEntry(title, url) });
                return;
            }

            MaybeAddEntry(title, url);
        }

        private void MaybeAddEntry(string title, string url)
        {
            var entries = History[title];
            if (entries.Any(e => e.Url == url)) { return; }

            entries.Add(BuildAndWriteInternetHistoryEntry(title, url));
        }

        private InternetHistoryEntry BuildAndWriteInternetHistoryEntry(string title, string url)
        {
            var type = DetermineType(title, url);
            var entry = InternetHistoryEntry.Entry(type, title, url);
            _data.Write(entry);
            return entry;
        }

        private LogType DetermineType(string title, string url)
        {
            foreach (var alertWord in _alertItems)
            {
                if (title.ToLower().Contains(alertWord)) { return LogType.Alert; }
                if (url.ToLower().Contains(alertWord)) { return LogType.Alert; }
            }
            return LogType.Info;
        }

        public void Stop(string reason)
        {
            _data.Write(InternetHistoryEntry.StopEntry(reason));
        }

        public void Start(string comment)
        {
            MaybeLoadCurrentHistoryData();
            _data.Write(InternetHistoryEntry.StartEntry(comment));
        }

        private void MaybeLoadCurrentHistoryData()
        {
            var data = _data.Read();
            if (data.Count == 0) { return; }

            var grouping = data.GroupBy(d => d.Title);
            foreach (var titleGroup in grouping)
            {
                History.Add(titleGroup.Key, titleGroup.ToList());
            }
        }

        private void InitializeAlertItems()
        {
            _alertItems = new List<string>();
            var ignoreItems = File.ReadAllLines(_config.GetAlertItemsFilePath());
            foreach (var item in ignoreItems) { _alertItems.Add(item); }
        }
    }
}
