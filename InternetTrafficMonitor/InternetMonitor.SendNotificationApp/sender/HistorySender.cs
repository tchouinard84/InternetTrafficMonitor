using InternetMonitor.Framework.Core.Data;
using InternetMonitor.Framework.Core.Models;
using InternetMonitor.SendNotificationApp.config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace InternetMonitor.SendNotificationApp.sender
{
    public class HistorySender : IHistorySender
    {
        private readonly EmailConfig _config;
        private readonly IHistoryData _historyData;

        public HistorySender()
        {
            _config = new EmailConfig();
            _historyData = new HistoryData();
        }

        public void MaybeSend(DateTime date)
        {
            var history = _historyData.Read(date);
            if (history.Count <= 0) { return; }

            Send(BuildMailMessage(history, date));
        }

        private static void Send(MailMessage message)
        {
            var client = new SmtpClient("escmail2", 25)
            {
                UseDefaultCredentials = true,
                EnableSsl = false
            };
            client.Send(message);
        }

        private MailMessage BuildMailMessage(IReadOnlyCollection<InternetHistoryEntry> history, DateTime date)
        {
            if (_config.IsTest) { return MailMessage(_config.DeveloperEmail, MessageBody(history), date); }

            var message = MailMessage(_config.To, MessageBody(history), date);
            MaybeAddCc(message);
            MaybeAddBcc(message);

            return message;
        }

        private MailMessage MailMessage(string emailTo, string messageBody, DateTime date)
        {
            var subject = string.Format(_config.Subject, date.ToShortDateString());

            return new MailMessage(_config.From, emailTo, subject, messageBody) { IsBodyHtml = true };
        }

        private void MaybeAddCc(MailMessage message)
        {
            var cc = _config.Cc;
            if (string.IsNullOrEmpty(cc)) { return; }
            message.CC.Add(cc);
        }

        private void MaybeAddBcc(MailMessage message)
        {
            var bcc = _config.Bcc;
            if (string.IsNullOrEmpty(bcc)) { return; }
            message.Bcc.Add(bcc);
        }

        private string MessageBody(IReadOnlyCollection<InternetHistoryEntry> history)
        {
            return $"<html><head>{HtmlStyle()}</head><body><div class=\"container\">"
                   + $"{GenerateHistoryHtml(history)}"
                   + $"</div>"
                   + $"</body></html>";
        }

        private static string GenerateHistoryHtml(IReadOnlyCollection<InternetHistoryEntry> history)
        {
            var html = new StringBuilder();

            var historyTypes = history.GroupBy(h => h.Type).ToList();

            html.Append(MaybeBuildAlertsHtmlTable(historyTypes));
            html.Append(BuildStartStopHtmlTable(historyTypes));
            html.Append("<hr>");
            html.Append(MaybeBuildGoogleSearchHtmlTable(history));
            html.Append(MaybeBuildYouTubeHtmlTable(history));
            html.Append(BuildHistoryHtmlTable(history));
            return html.ToString();
        }

        private static string BuildHistoryHtmlTable(IEnumerable<InternetHistoryEntry> history)
        {
            var html = new StringBuilder("<div><h3>Internet History Log</h3>"
                                         + "<table><tr><th>Time</th><th>Type</th><th>Title</th><th>Url</th></tr>");

            foreach (var entry in history)
            {
                if (entry.Type == LogType.Start || entry.Type == LogType.Stop) { continue; }

                html.Append($"<tr><td>{entry.TimeStamp.ToShortTimeString()}</td>"
                            + $"<td>{entry.Type}</td>"
                            + $"<td>{entry.Title}</td>"
                            + $"<td><a href=\"{entry.Url}\" target=\"_blank\">{entry.GetDomainName()}</a></td></tr>");
            }
            html.Append("</table></div>");
            return html.ToString();
        }

        private static string MaybeBuildGoogleSearchHtmlTable(IEnumerable<InternetHistoryEntry> history)
        {
            var googleSearches = history.Where(h => h.IsGoogleSearch()).Select(GoogleSearch.ValueOf).ToList();
            if (googleSearches.Count < 1) { return string.Empty; }

            var googleSearchHistory = googleSearches.GroupBy(gs => new { gs.Type, gs.SearchQuery }).Select(gs => new
            {
                gs.Key.Type,
                gs.Key.SearchQuery,
                ProgressiveClicks = gs.Count(),
                AllSafe = gs.All(x => x.IsSafe)
            });

            var html = new StringBuilder("<div><h3>Google Searches</h3>"
                                         + "<table><tr><th>Type</th><th>Search Query</th><th>Progressive Clicks</th><th>All Safe Searches</th></tr>");

            foreach (var googleSearch in googleSearchHistory)
            {
                html.Append($"<tr><td>{googleSearch.Type}</td>"
                            + $"<td>{googleSearch.SearchQuery}</td>"
                            + $"<td>{googleSearch.ProgressiveClicks}</td>"
                            + $"<td>{googleSearch.AllSafe}</td></tr>");
            }
            html.Append("</table></div><hr>");
            return html.ToString();
        }

        private static string MaybeBuildYouTubeHtmlTable(IEnumerable<InternetHistoryEntry> history)
        {
            var youTubes = history.Where(h => h.IsYouTube()).Select(YouTube.ValueOf).ToList();
            if (youTubes.Count < 1) { return string.Empty; }

            var youTubeHistory = youTubes.GroupBy(ut => new { ut.Type, ut.Title, ut.SearchQuery }).Select(utg => new
            {
                utg.Key.Type,
                utg.Key.Title,
                utg.Key.SearchQuery
            });

            var html = new StringBuilder("<div><h3>YouTube</h3>"
                                         + "<table><tr><th>Type</th><th>Title</th><th>Search Query</th></tr>");

            foreach (var youTube in youTubeHistory)
            {
                html.Append($"<tr><td>{youTube.Type}</td>"
                            + $"<td>{youTube.Title}</td>"
                            + $"<td>{youTube.SearchQuery}</td></tr>");
            }
            html.Append("</table></div><hr>");
            return html.ToString();
        }

        private static string BuildStartStopHtmlTable(IReadOnlyCollection<IGrouping<LogType, InternetHistoryEntry>> historyTypes)
        {
            var starts = GetEntries(historyTypes, LogType.Start);
            var stops = GetEntries(historyTypes, LogType.Stop);

            var startStops = new Dictionary<InternetHistoryEntry, InternetHistoryEntry>();

            for (var i = starts.Count-1; i >= 0; i--)
            {
                var start = starts[i];
                var stop = stops.FirstOrDefault(s => s.TimeStamp >= start.TimeStamp);
                stops.Remove(stop);
                startStops.Add(start, stop);
            }

            var orderedStartStops = startStops.OrderBy(ss => ss.Key.TimeStamp);

            var html = new StringBuilder("<div><table><tr><th>Start</th><th>Comment</th><th>Stop</th><th>Reason</th></tr>");
            foreach (var startStop in orderedStartStops)
            {
                var stopTime = startStop.Value == null ? "?" : startStop.Value.TimeStamp.ToShortTimeString();
                var stopReason = startStop.Value == null ? "?" : startStop.Value.Title;
                html.Append($"<tr><td>{startStop.Key.TimeStamp.ToShortTimeString()}</td>"
                            + $"<td>{BuildStartComment(startStop.Key.Title)}</td>"
                            + $"<td>{stopTime}</td>"
                            + $"<td>{stopReason}</td></tr>");
            }

            html.Append("</table></div>");
            return html.ToString();
        }

        private static string BuildStartComment(string startComment)
        {
            return $"<pre>{startComment}</pre>";
        }

        private static string MaybeBuildAlertsHtmlTable(IEnumerable<IGrouping<LogType, InternetHistoryEntry>> historyTypes)
        {
            var alertEntries = GetEntries(historyTypes, LogType.Alert);
            if (alertEntries.Count <= 0) { return string.Empty; }

            var html = new StringBuilder("<div><h2>ALERTS</h2>"
                                  + "<table><tr><th>Time</th><th>Title</th><th>Url</th></tr>");

            foreach (var alert in alertEntries)
            {
                html.Append($"<tr><td>{alert.TimeStamp.ToShortTimeString()}</td>"
                            + $"<td>{alert.Title}</td>"
                            + $"<td><a href=\"{alert.Url}\" target=\"_blank\">{alert.GetDomainName()}</a></td></tr>");
            }

            html.Append("</table></div><hr>");
            return html.ToString();
        }

        private static List<InternetHistoryEntry> GetEntries(
            IEnumerable<IGrouping<LogType, InternetHistoryEntry>> historyTypes, LogType type)
        {
            var typeGroup = historyTypes.FirstOrDefault(ht => ht.Key == type);
            if (typeGroup == null) { return new List<InternetHistoryEntry>(); }

            return (from entries in typeGroup select entries).ToList();
        }

        private static string HtmlStyle()
        {
            return @"<style>
.container {
  padding-right: 15px;
  padding-left: 15px;
  margin-right: auto;
  margin-left: auto;
}
@media (min-width: 768px) {
  .container {
    width: 750px;
  }
}
@media (min-width: 992px) {
  .container {
    width: 970px;
  }
}
@media (min-width: 1200px) {
  .container {
    width: 1170px;
  }
}
body {
	font-family: arial, sans-serif;
}
table {
	border-collapse: collapse;
}
td, th {
	border: 1px solid #adadad;
	text-align: left;
	padding: 8px;
}
tr:nth-child(even) {
	background-color: #dddddd;
}
</style>";
        }
    }
}
