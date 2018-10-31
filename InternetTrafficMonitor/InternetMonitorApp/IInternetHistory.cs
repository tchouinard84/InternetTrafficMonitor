namespace InternetMonitorApp
{
    public interface IInternetHistory
    {
        void MaybeAddHistory(string title, string url);
        void Stop(string reason);
        void Start(string comment);
    }
}