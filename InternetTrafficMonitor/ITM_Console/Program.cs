using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace ITM_Console
{
    public class Program
    {
        static void Main(string[] args)
        {
            var inappropriateContent = File.ReadLines(@"C:\Users\tchouina\Personal\InternetMonitor\inappropriate_content.txt");
            var inappropriateWords = new List<string>();

            foreach (var word in inappropriateContent) { inappropriateWords.Add(" " + word + " "); }

            var browsers = new List<IUrlRetriever>()
            {
                new ChromeUrlRetriever(),
                new InternetExplorerUrlRetriever()
            };

            while (true)
            {
                Thread.Sleep(1000);

                foreach (var browser in browsers)
                {
                    var url = browser.MaybeGetCurrentBrowserUrl();
                    if (url == null) { continue; }
                    Console.WriteLine(url);
                    Console.WriteLine("---------------------------------------------------------------");

                    var process = browser.TheProcess;
                    /*
                    Console.WriteLine("MachineName: " + process.MachineName);
                    Console.WriteLine("MainWindowTitle: " + process.MainWindowTitle);
                    Console.WriteLine("ProcessName: " + process.ProcessName);
                    Console.WriteLine("UserProcessorTime: " + process.UserProcessorTime);
                    Console.WriteLine("HandleCount: " + process.HandleCount);
                    Console.WriteLine("Number of Threads: " + process.Threads.Count);
                    Console.WriteLine("process: " + process);
                    */

                    foreach (var word in inappropriateWords)
                    {
                        if (!url.Contains(word)) { continue; }
                        Console.WriteLine(Environment.NewLine + "@@@@ ---- ALERT - Inappropriate Content -Detected: " + word + " ---- @@@@" + Environment.NewLine);
                        process.CloseMainWindow();
                    }
                }
            }

            /*
            foreach (var process in Process.GetProcessesByName("chrome"))
            {
                if (process == null) { throw new ArgumentNullException("process"); }
                if (process.MainWindowHandle == IntPtr.Zero) { return; }

                var mainForm = AutomationElement.FromHandle(process.MainWindowHandle);
                Console.WriteLine(mainForm.GetCurrentPropertyValue(AutomationProperty.LookupById(0)));
            }
            */
        }
    }
}
