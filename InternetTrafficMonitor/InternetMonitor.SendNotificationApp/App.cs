using InternetMonitor.SendNotificationApp.sender;
using System;

namespace InternetMonitor.SendNotificationApp
{
    public class App
    {
        private readonly IHistorySender _sender;

        public App(IHistorySender sender)
        {
            _sender = sender;
        }

        public void Run()
        {
            _sender.MaybeSend(DateTime.Today);
        }
    }
}
