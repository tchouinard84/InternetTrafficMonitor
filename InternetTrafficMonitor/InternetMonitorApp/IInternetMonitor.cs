namespace InternetMonitorApp
{
    public interface IInternetMonitor
    {
        void Start(string comment);
        void Stop(string reason);
    }
}