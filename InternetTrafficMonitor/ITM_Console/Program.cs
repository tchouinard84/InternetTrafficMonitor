using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace ITM_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true) { GetCurrentBrowsersUrl(); }
            //ListenForNetworkChanges();
        }

        private static void GetCurrentBrowsersUrl()
        {
            foreach (Process process in Process.GetProcessesByName("firefox"))
            {
                string url = GetFirefoxUrl(process);
                if (url == null)
                    continue;

                Console.WriteLine("FF Url for '" + process.MainWindowTitle + "' is " + url);
            }

            foreach (Process process in Process.GetProcessesByName("iexplore"))
            {
                string url = GetInternetExplorerUrl(process);
                if (url == null)
                    continue;

                Console.WriteLine("IE Url for '" + process.MainWindowTitle + "' is " + url);
            }

            foreach (Process process in Process.GetProcessesByName("chrome"))
            {
                string url = GetChromeUrl(process);
                if (url == null)
                    continue;

                Console.WriteLine("CH Url for '" + process.MainWindowTitle + "' is " + url);
            }
        }

        public static string GetChromeUrl(Process process)
        {
            if (process == null)
                throw new ArgumentNullException("process");

            if (process.MainWindowHandle == IntPtr.Zero)
                return null;

            AutomationElement element = AutomationElement.FromHandle(process.MainWindowHandle);
            if (element == null)
                return null;
            AutomationElement edit = element.FindFirst(TreeScope.Subtree,
                new AndCondition(
                    new PropertyCondition(AutomationElement.NameProperty, "address and search bar", PropertyConditionFlags.IgnoreCase),
                    new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit)));

            return ((ValuePattern)edit.GetCurrentPattern(ValuePattern.Pattern)).Current.Value as string;
        }

        public static string GetInternetExplorerUrl(Process process)
        {
            if (process == null)
                throw new ArgumentNullException("process");

            if (process.MainWindowHandle == IntPtr.Zero)
                return null;

            AutomationElement element = AutomationElement.FromHandle(process.MainWindowHandle);
            if (element == null)
                return null;

            AutomationElement rebar = element.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, "ReBarWindow32"));
            if (rebar == null)
                return null;

            AutomationElement edit = rebar.FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));

            return ((ValuePattern)edit.GetCurrentPattern(ValuePattern.Pattern)).Current.Value as string;
        }

        public static string GetFirefoxUrl(Process process)
        {
            if (process == null)
                throw new ArgumentNullException("process");

            if (process.MainWindowHandle == IntPtr.Zero)
                return null;

            AutomationElement element = AutomationElement.FromHandle(process.MainWindowHandle);
            if (element == null)
                return null;

            AutomationElement doc = element.FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Document));
            if (doc == null)
                return null;

            return ((ValuePattern)doc.GetCurrentPattern(ValuePattern.Pattern)).Current.Value as string;
        }

        /*

        private static void ListenForNetworkChanges()
        {
            while (true)
            {
                NetworkChange.NetworkAvailabilityChanged += NetworkAvailabilityChanged;
                NetworkChange.NetworkAddressChanged += NetworkAddressChanged;
            }
        }

        private static void NetworkAddressChanged(object sender, EventArgs e)
        {
            Console.WriteLine("Current IP Addresses:");
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (UnicastIPAddressInformation addr in ni.GetIPProperties().UnicastAddresses)
                {
                    Console.WriteLine("    - {0} (lease expires {1})", addr.Address, DateTime.Now + new TimeSpan(0, 0, (int)addr.DhcpLeaseLifetime));
                }
            }
        }

        private static void NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            if (e.IsAvailable)
            {
                Console.WriteLine("Network Available");
            }
            else
            {
                Console.WriteLine("Network Unavailable");
            }
        }
        */
    }
}
