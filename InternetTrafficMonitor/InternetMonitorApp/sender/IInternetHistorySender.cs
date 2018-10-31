using System;

namespace InternetMonitorApp.sender
{
    public interface IInternetHistorySender
    {
        void Send(DateTime date);
    }
}