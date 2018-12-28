namespace InternetMonitor.Framework.Core
{
    public interface IInternetHistory
    {
        void Start(string comment);
        void Stop(string reason);
        bool Contains(string url, string title);
        void WriteEntry(string title, string url);
    }
}