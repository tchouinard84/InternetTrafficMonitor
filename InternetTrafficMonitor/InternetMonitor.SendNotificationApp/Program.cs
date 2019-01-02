using InternetMonitor.SendNotificationApp.sender;
using NLog;
using System;

namespace InternetMonitor.SendNotificationApp
{
    public class Program
    {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            try
            {
                var sender = new HistorySender();
                sender.MaybeSend(DateTime.Today);
            }
            catch (Exception e)
            {
                log.Error(e, "Unknown Error");
            }
        }
    }
}
