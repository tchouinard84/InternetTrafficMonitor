using InternetMonitor.SendNotificationApp.sender;
using System;

namespace InternetMonitor.SendNotificationApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var sender = new HistorySender();
            sender.MaybeSend(DateTime.Today);
        }
    }
}
