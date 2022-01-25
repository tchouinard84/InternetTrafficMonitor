using InternetMonitor.Framework.Core.models;

namespace InternetMonitor.Framework.Core
{
    public interface IInternetHistory
    {
        void Start(string comment);
        void Stop(string reason);
        void Comment(string comment);
        bool Contains(string url, string title);
        void WriteEntry(LogType type, string title, string url);
    }
}