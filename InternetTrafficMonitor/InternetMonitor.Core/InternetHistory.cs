using InternetMonitor.Core.config;
using InternetMonitor.Core.data;
using InternetMonitor.Core.models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace InternetMonitor.Core
{
    public class InternetHistory : IInternetHistory
    {
        private readonly IAppConfig _config;
        private readonly IHistoryData _data;

        public InternetHistory()
        {
            _config = new AppConfig();
            _data = new HistoryData();
        }

        public InternetHistory(IOptions<AppConfig> configOptions, IHistoryData data)
        {
            _config = configOptions.Value;
            _data = data;
        } 

        public void Start(string comment)
        {
            _data.Write(InternetHistoryEntry.StartEntry(comment));
        }

        public void Stop(string reason)
        {
            _data.Write(InternetHistoryEntry.StopEntry(reason));
        }

        public bool Contains(string url, string title)
        {
            var data = _data.Read();
            if (data.Count == 0) { return false; }
            if (data.Any(d => d.Url == url)) { return true; }
            return data.Any(d => d.Title == title);
        }

        public void WriteEntry(string title, string url)
        {
            var type = DetermineType(title, url);
            var entry = InternetHistoryEntry.Entry(type, title, url);
            _data.Write(entry);
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

        private IEnumerable<string> GetAlertItems() => File.ReadAllLines(_config.GetAlertItemsFilePath()).ToList();
    }
}
