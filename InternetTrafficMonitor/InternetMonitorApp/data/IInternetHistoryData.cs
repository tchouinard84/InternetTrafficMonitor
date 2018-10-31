using InternetMonitorApp.models;
using System;
using System.Collections.Generic;

namespace InternetMonitorApp.data
{
    public interface IInternetHistoryData
    {
        void Write(InternetHistoryEntry entry);
        IReadOnlyCollection<InternetHistoryEntry> Read();
        IReadOnlyCollection<InternetHistoryEntry> Read(DateTime date);
    }
}