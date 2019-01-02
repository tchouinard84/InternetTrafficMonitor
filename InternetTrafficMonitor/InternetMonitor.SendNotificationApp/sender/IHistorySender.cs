using System;

namespace InternetMonitor.SendNotificationApp.sender
{
    public interface IHistorySender
    {
        void MaybeSend(DateTime date);
    }
}