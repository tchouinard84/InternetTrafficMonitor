using InternetMonitor.Framework.Core.data;
using InternetMonitor.Framework.Core.models;
using System.Linq;

namespace InternetMonitor.Framework.Core
{
    public class InternetHistory : IInternetHistory
    {
        private readonly IHistoryData _data;

        public InternetHistory()
        {
            _data = new HistoryData();
        }

        public void Start(string comment)
        {
            _data.Write(InternetHistoryEntry.StartEntry(comment));
        }

        public void Stop(string reason)
        {
            _data.Write(InternetHistoryEntry.StopEntry(reason));
        }

        public void Comment(string comment)
        {
            _data.Write(InternetHistoryEntry.CommentEntry(comment));
        }

        public bool Contains(string url, string title)
        {
            var data = _data.Read();
            if (data.Count == 0) { return false; }
            if (data.Any(d => d.Url == url)) { return true; }
            return data.Any(d => d.Title == title);
        }

        public void WriteEntry(LogType type, string title, string url)
        {
            if (Contains(url, title)) { return; }
            var entry = InternetHistoryEntry.Entry(type, title, url);
            _data.Write(entry);
        }
    }
}
