using InternetMonitor.data;
using InternetMonitor.models;
using System.Collections.Generic;
using System.Linq;

namespace InternetMonitor
{
    public class InternetHistory
    {
        private readonly IEnumerable<string> _alertWords;
        private readonly InternetHistoryData _data;
        public Dictionary<string, List<InternetHistoryEntry>> History { get; private set; }

        public InternetHistory(IEnumerable<string> alertWords)
        {
            _alertWords = alertWords;
            _data = new InternetHistoryData();
            History = new Dictionary<string, List<InternetHistoryEntry>>();
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
            foreach (var alertWord in _alertWords)
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

        public void Start()
        {
            MaybeLoadCurrentHistoryData();
            _data.Write(InternetHistoryEntry.StartEntry());
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
    }
}
