using InternetMonitor.Framework.Core.Data;
using InternetMonitor.Framework.Core.Models;
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

        public void WriteEntry(LogType type, string title, string url)
        {
            if (Contains(title, url)) { return; }
            var entry = InternetHistoryEntry.Entry(type, title, url);
            _data.Write(entry);
        }

        private bool Contains(string title, string url)
        {
            var data = _data.Read();
            if (data.Count == 0) { return false; }
            if (data.Any(d => d.Url == url)) { return true; }
            return data.Any(d => d.Title == title);
        }
    }
}
