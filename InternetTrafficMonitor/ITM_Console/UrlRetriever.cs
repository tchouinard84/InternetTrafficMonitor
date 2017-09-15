using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace ITM_Console
{
    public abstract class UrlRetriever : IUrlRetriever
    {
        private string currentUrl;
        public Browser TheBrowser { get; protected set; }

        public string MaybeGetCurrentBrowserUrl()
        {
            foreach (var process in Process.GetProcessesByName(TheBrowser.ToString()))
            {
                var url = MaybeGetUrl(process);
                if (url == null) { continue; }

                if (url.Equals(currentUrl)) { return null; }

                currentUrl = url;
                return TheBrowser.ToString() + " Url for '" + process.MainWindowTitle + "' is " + url;
            }
            return null;
        }

        public string MaybeGetUrl(Process process)
        {
            var element = GetElement(process);
            if (element == null) { return null; }
            return GetUrl(element);
        }

        private static AutomationElement GetElement(Process process)
        {
            if (process == null) { throw new ArgumentNullException("process"); }
            if (process.MainWindowHandle == IntPtr.Zero) { return null; }

            return AutomationElement.FromHandle(process.MainWindowHandle);
        }

        protected abstract string GetUrl(AutomationElement element);
    }
}
