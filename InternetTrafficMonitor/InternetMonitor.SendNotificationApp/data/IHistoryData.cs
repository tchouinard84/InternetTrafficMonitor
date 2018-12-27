using InternetMonitor.SendNotificationApp.models;
using System;
using System.Collections.Generic;

namespace InternetMonitor.SendNotificationApp.data
{
    public interface IHistoryData
    {
        void Write(InternetHistoryEntry entry);
        IReadOnlyCollection<InternetHistoryEntry> Read();
        IReadOnlyCollection<InternetHistoryEntry> Read(DateTime date);
    }
}