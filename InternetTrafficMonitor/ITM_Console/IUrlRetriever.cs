using System.Diagnostics;

namespace ITM_Console
{
    public interface IUrlRetriever
    {
        Browser TheBrowser { get; }
        string MaybeGetCurrentBrowserUrl();
    }
}