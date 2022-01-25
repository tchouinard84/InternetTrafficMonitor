namespace InternetMonitor.Framework.Core
{
    public interface IInternetBlocker
    {
        void IgnoreItem(string item);
        void BlockItem(string item);
        void CheckProcesses();
    }
}
