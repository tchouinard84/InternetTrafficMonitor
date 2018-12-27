using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace InternetMonitor.SendNotificationApp.models
{
    public class GoogleSearch
    {
        public DateTime Timestamp { get; private set; }
        public GoogleSearchType Type { get; private set; }
        public string SearchQuery { get; private set; }
        public bool IsSafe { get; private set; }
        public string Url { get; private set; }

        public static GoogleSearch ValueOf(InternetHistoryEntry entry)
        {
            var urlParams = GetParams(entry.Url);
            return new GoogleSearch
            {
                Timestamp = entry.TimeStamp,
                Type = DetermineType(urlParams),
                SearchQuery = DetermineSearchQuery(urlParams),
                IsSafe = DetermineIsSafe(urlParams),
                Url = entry.Url
            };
        }

        private static string DetermineSearchQuery(Dictionary<string, string> urlParams)
        {
            if (!urlParams.ContainsKey("q")) { return string.Empty; }
            return urlParams["q"].Replace("+", " ");
        }

        private static bool DetermineIsSafe(IReadOnlyDictionary<string, string> urlParams)
        {
            if (!urlParams.ContainsKey("safe")) { return false; }
            return urlParams["safe"] == "active";
        }

        private static GoogleSearchType DetermineType(IReadOnlyDictionary<string, string> urlParams)
        {
            urlParams.TryGetValue("tbm", out var searchType);
            return GoogleSearchType.FromString(searchType);
        }

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
