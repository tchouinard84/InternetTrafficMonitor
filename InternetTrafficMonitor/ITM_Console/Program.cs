using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace ITM_Console
{
    public class Program
    {
        static void Main(string[] args)
        {
            //while (true) { GetCurrentBrowsersUrl(); }
            //ListenForNetworkChanges();

            var browsers = new List<IUrlRetriever>()
            {
                new ChromeUrlRetriever(),
                new InternetExplorerUrlRetriever()
            };

            while (true)
            {
                Thread.Sleep(2000);
                foreach (var browser in browsers)
                {
                    var url = browser.MaybeGetCurrentBrowserUrl();
                    if (url == null) { continue; }
                    Console.WriteLine(url);
                    Console.WriteLine("---------------------------------------------------------------");
                }
            }
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
