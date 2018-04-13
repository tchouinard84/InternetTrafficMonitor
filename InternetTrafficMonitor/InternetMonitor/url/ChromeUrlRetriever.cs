using System;
using System.Diagnostics;
using System.Windows.Automation;

namespace InternetMonitor.url
{
    public static class ChromeUrlRetriever
    {
        public static string GetUrl(Process process)
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

        private static string GetUrl(AutomationElement element)
        {
            if (element == null) { return null; }

            var edit = element.FindFirst(TreeScope.Subtree,
                new AndCondition(
                    new PropertyCondition(AutomationElement.NameProperty, "address and search bar", PropertyConditionFlags.IgnoreCase),
                    new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit)));
            if (edit == null) { return null; }

            return ((ValuePattern)edit.GetCurrentPattern(ValuePattern.Pattern)).Current.Value as string;
        }
    }
}
