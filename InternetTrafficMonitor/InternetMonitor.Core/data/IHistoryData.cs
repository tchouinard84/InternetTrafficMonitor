using System;
using System.Collections.Generic;
using InternetMonitor.Core.models;

namespace InternetMonitor.Core.data
{
    public interface IHistoryData
    {
        void Write(InternetHistoryEntry entry);
        IReadOnlyCollection<InternetHistoryEntry> Read();
        IReadOnlyCollection<InternetHistoryEntry> Read(DateTime date);
    }
}