using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;

namespace InternetMonitor.models
{
    public class InternetHistoryEntry
    {
        public DateTime TimeStamp { get; }

        [JsonProperty("Type")]
        [JsonConverter(typeof(LogTypeConverter))]
        public LogType Type { get; set; }
        [JsonProperty("Title")]
        public string Title { get; }
        [JsonProperty("Url")]
        public string Url { get; }

        //[JsonConstructor]
        //public InternetHistoryEntry() { }
        [JsonConstructor]
        public InternetHistoryEntry(DateTime timeStamp, LogType type, string title, string url)
        {
            TimeStamp = timeStamp;
            Type = type;
            Title = title;
            Url = url;
        }

        private InternetHistoryEntry(LogType type, string title)
            : this(DateTime.Now, type, title, string.Empty) { }

        private InternetHistoryEntry(LogType type, string title, string url)
            : this(DateTime.Now, type, title, url) { }

        public static InternetHistoryEntry StartEntry(string comment) =>
            new InternetHistoryEntry(LogType.Start, comment);

        public static InternetHistoryEntry StopEntry(string reason) =>
            new InternetHistoryEntry(LogType.Stop, reason);

        public static InternetHistoryEntry Entry(LogType type, string title, string url) =>
            new InternetHistoryEntry(type, title, url);

        public bool IsGoogleSearch()
        {
            var domain = ExtractDomainFromUrl();
            if (domain != "www.google.com") { return false; }

            var pattern = @"^((http[s]?|ftp):\/)?\/?([^:\/\s]+)((\/\w+)*\/)([\w\-\.]+[^#?\s]+)(.*)?(#[\w\-]+)?$";
            var match = Regex.Match(Url, pattern);
            if (!match.Success) { return false; }
            return match.Groups[6].Value == "search";
        }

        public string GetDomainName() => ExtractDomainFromUrl();

        private string ExtractDomainFromUrl()
        {
            if (string.IsNullOrEmpty(Url)) { return string.Empty; }

            var pattern = @"^((http[s]?|ftp):\/)?\/?([^:\/\s]+)((\/\w+)*\/)([\w\-\.]+[^#?\s]+)(.*)?(#[\w\-]+)?$";
            var match = Regex.Match(Url, pattern);
            if (!match.Success) { return string.Empty; }

            return match.Groups[3].Value;
        }

        public bool IsYouTube()
        {
            if (string.IsNullOrEmpty(Url)) { return false; }

            var match = Regex.Match(Url, @"^((http[s]?|ftp):\/)?\/?([^:\/\s]+)((\/\w+)*\/)");
            if (!match.Success) { return false; }
            if (match.Groups[3].Value != "www.youtube.com") { return false; }
            return true;
        }
    }
}
