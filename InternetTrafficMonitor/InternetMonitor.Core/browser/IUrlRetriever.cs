using System.Diagnostics;

namespace InternetMonitor.Core.browser
{
    public interface IUrlRetriever
    {
        string Browser { get; }

        string GetUrl(Process process);
    }
}