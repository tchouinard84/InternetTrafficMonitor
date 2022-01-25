using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace InternetMonitor.Framework.Core.Models
{
    public class YouTube
    {
        public DateTime Timestamp { get; private set; }
        public YouTubeType Type { get; set; }
        public string Title { get; set; }
        public string SearchQuery { get; set; }
        public string Url { get; set; }

        public static YouTube ValueOf(InternetHistoryEntry entry)
        {
            var urlParams = GetParams(entry.Url).FirstOrDefault();
            return new YouTube
            {
                Timestamp = entry.TimeStamp,
                Type = DetermineType(urlParams.Key),
                Title = entry.Title,
                SearchQuery = DetermineSearchQuery(urlParams),
                Url = entry.Url
            };
        }

        private static string DetermineSearchQuery(KeyValuePair<string, string> urlParams)
        {
            if (urlParams.Key != YouTubeType.Search.Value) { return string.Empty; }
            return urlParams.Value.Replace("+", " ");
        }

        private static YouTubeType DetermineType(string param) => YouTubeType.FromString(param);

        private static Dictionary<string, string> GetParams(string url)
        {
            var matches = Regex.Matches(url, @"[\?&](([^&=]+)=([^&=#]*))", RegexOptions.Compiled);
            return matches.Cast<Match>().ToDictionary(
                m => Uri.UnescapeDataString(m.Groups[2].Value),
                m => Uri.UnescapeDataString(m.Groups[3].Value)
            );
        }
    }
}
