using System;
using System.Collections.Generic;
using InternetMonitor.Framework.Core.models;

namespace InternetMonitor.Framework.Core.data
{
    public interface IHistoryData
    {
        void Write(InternetHistoryEntry entry);
        IReadOnlyCollection<InternetHistoryEntry> Read();
        IReadOnlyCollection<InternetHistoryEntry> Read(DateTime date);
    }
}