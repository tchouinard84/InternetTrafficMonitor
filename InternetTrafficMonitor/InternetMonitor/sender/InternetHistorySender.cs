using InternetMonitor.data;
using InternetMonitor.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace InternetMonitor.sender
{
    public class InternetHistorySender
    {
        public void Send(DateTime date)
        {
            var data = new InternetHistoryData();
            var history = data.Read(date);

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
            var message = new MailMessage
            {
                From = new MailAddress("Timothy.Chouinard@emp-corp.com"),
                Subject = $"Internet History for {date.ToShortDateString()}",
                IsBodyHtml = true,
                Body = MessageBody(history)
            };
            message.To.Add("tchou84@yahoo.com");//ConfigurationManager.AppSettings["sendHistoryTo"]);
            return message;
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

            html.Append(BuildAlertsHtmlTable(historyTypes));
            html.Append("<hr>");
            html.Append(BuildStartStopHtmlTable(historyTypes));
            html.Append("<hr>");
            html.Append(BuildGoogleSearchHtmlTable(history));
            html.Append("<hr>");
            html.Append(BuildHistoryHtmlTable(history));
            return html.ToString();
        }

        private static string BuildHistoryHtmlTable(IEnumerable<InternetHistoryEntry> history)
        {
            var html = new StringBuilder("<div><h3>Internet History Log</h3>"
                                         + "<table><tr><th>Time</th><th>Type</th><th>Title</th><th>Url</th></tr>");

            foreach (var entry in history)
            {
                html.Append($"<tr><td>{entry.TimeStamp.ToShortTimeString()}</td>"
                            + $"<td>{entry.Type}</td>"
                            + $"<td>{entry.Title}</td>"
                            + $"<td><a href=\"{entry.Url}\" target=\"_blank\">{entry.GetDomainName()}</a></td></tr>");
            }
            html.Append("</table></div>");
            return html.ToString();
        }

        private static string BuildGoogleSearchHtmlTable(IEnumerable<InternetHistoryEntry> history)
        {
            var googleSearches = history.Where(h => h.IsGoogleSearch()).Select(GoogleSearch.ValueOf).ToList();
            if (googleSearches.Count < 1) { return string.Empty; }

            var foo = googleSearches.GroupBy(gs => new { gs.Type, gs.SearchQuery }).Select(gs => new
            {
                gs.Key.Type,
                gs.Key.SearchQuery,
                ProgressiveClicks = gs.Count(),
                AllSafe = gs.All(x => x.IsSafe)
            });

            var html = new StringBuilder("<div><h3>Google Searches</h3>"
                                         + "<table><tr><th>Type</th><th>Search Query</th><th>Progressive Clicks</th><th>All Safe Searches</th></tr>");

            foreach (var googleSearch in foo)
            {
                html.Append($"<tr><td>{googleSearch.Type}</td>"
                            + $"<td>{googleSearch.SearchQuery}</td>"
                            + $"<td>{googleSearch.ProgressiveClicks}</td>"
                            + $"<td>{googleSearch.AllSafe}</td></tr>");
            }
            html.Append("</table></div>");
            return html.ToString();
        }

        private static string BuildStartStopHtmlTable(IReadOnlyCollection<IGrouping<LogType, InternetHistoryEntry>> historyTypes)
        {
            var starts = GetEntries(historyTypes, LogType.Start);
            var stops = GetEntries(historyTypes, LogType.Stop);

            var length = starts.Count <= stops.Count ? starts.Count : stops.Count;

            var html = new StringBuilder("<div><table><tr><th>Start</th><th>Stop</th><th>Reason</th></tr>");
            for (var i = 0; i < length; i++)
            {
                html.Append($"<tr><td>{starts[i].TimeStamp.ToShortTimeString()}</td>"
                            + $"<td>{stops[i].TimeStamp.ToShortTimeString()}</td>"
                            + $"<td>{stops[i].Title}</td></tr>");
            }

            html.Append("</table></div>");
            return html.ToString();
        }

        private static string BuildAlertsHtmlTable(IEnumerable<IGrouping<LogType, InternetHistoryEntry>> historyTypes)
        {
            var html = new StringBuilder("<div><h2>ALERTS</h2>"
                                  + "<table><tr><th>Time</th><th>Title</th><th>Url</th></tr>");

            foreach (var alert in GetEntries(historyTypes, LogType.Alert))
            {
                html.Append($"<tr><td>{alert.TimeStamp.ToShortTimeString()}</td>"
                            + $"<td>{alert.Title}</td>"
                            + $"<td><a href=\"{alert.Url}\" target=\"_blank\">{alert.GetDomainName()}</a></td></tr>");
            }

            html.Append("</table></div>");
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
