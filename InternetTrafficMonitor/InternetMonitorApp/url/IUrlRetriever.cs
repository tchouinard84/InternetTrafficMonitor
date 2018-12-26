using System.Diagnostics;

namespace InternetMonitorApp.url
{
    public interface IUrlRetriever
    {
        string GetUrl(Process process);
    }
}