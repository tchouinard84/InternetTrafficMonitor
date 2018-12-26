using System.Diagnostics;

namespace InternetMonitor.url
{
    public interface IUrlRetriever
    {
        string Browser { get; }

        string GetUrl(Process process);
    }
}