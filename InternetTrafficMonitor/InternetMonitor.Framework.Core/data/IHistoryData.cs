using InternetMonitor.Framework.Core.Models;
using System;
using System.Collections.Generic;

namespace InternetMonitor.Framework.Core.Data
{
    public interface IHistoryData
    {
        void Write(InternetHistoryEntry entry);
        IReadOnlyCollection<InternetHistoryEntry> Read();
        IReadOnlyCollection<InternetHistoryEntry> Read(DateTime date);
    }
}