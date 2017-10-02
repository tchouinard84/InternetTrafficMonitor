using System.Diagnostics;

namespace ITM_Console
{
    public interface IUrlRetriever
    {
        Browser TheBrowser { get; }
        Process TheProcess { get; }
        string MaybeGetCurrentBrowserUrl();
    }
}