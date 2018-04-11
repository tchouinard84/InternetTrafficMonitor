using System.Collections.Generic;

namespace InternetMonitor
{
    public class WebsiteHistory
    {
        private Dictionary<string, List<WebUrl>> _history;

        public WebsiteHistory()
        {
            _history = new Dictionary<string, List<WebUrl>>();
        }

        public bool Contains(string title) => _history.ContainsKey(title);

        public void Add(string title)
        {
            _history.Add(title, new List<WebUrl>());
        }
    }
}
