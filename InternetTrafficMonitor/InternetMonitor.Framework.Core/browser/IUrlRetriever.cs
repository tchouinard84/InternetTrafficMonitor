using System.Diagnostics;

namespace InternetMonitor.Framework.Core.Browser
{
    public interface IUrlRetriever
    {
        string Browser { get; }

        string GetUrl(Process process);
    }
}