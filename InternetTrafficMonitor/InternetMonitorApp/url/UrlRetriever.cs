using System;
using System.Diagnostics;
using System.Windows.Automation;

namespace InternetMonitorApp.url
{
    public abstract class UrlRetriever : IUrlRetriever
    {
        public string GetUrl(Process process)
        {
            var element = GetElement(process);
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
