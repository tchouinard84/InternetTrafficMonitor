using System.Diagnostics;

namespace InternetMonitor.Framework.Core.browser
{
    public interface IUrlRetriever
    {
        string Browser { get; }

        string GetUrl(Process process);
    }
}