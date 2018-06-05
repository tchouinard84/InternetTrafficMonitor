using InternetMonitor.sender;
using System;
using System.Threading;

namespace InternetMonitor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //new InternetHistoryData().Read(DateTime.Today).Dump();

            RunInternetMonitor();
        }

        private static void RunInternetMonitor()
        {
            var monitor = new InternetMonitor();

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                monitor.Start();
            }).Start();

            do
            {
                Console.WriteLine($"Commands: {InputCommand.Exit}{InputCommand.SendAndExit}");
                var input = Console.ReadLine();

                if (string.IsNullOrEmpty(input)) { continue; }

                if (input == InputCommand.Exit.Value)
                {
                    monitor.Stop(GetReason());
                    break;
                }

                if (input == InputCommand.SendAndExit.Value)
                {
                    monitor.Stop("End of Day");
                    SendEmail();
                    break;
                }
            }
            while (true);
        }

        private static void SendEmail()
        {
            var sender = new InternetHistorySender();
            sender.Send(DateTime.Today);
        }

        private static string GetReason()
        {
            Console.WriteLine($"Please enter a reason: ");
            var reason = Console.ReadLine();
            if (string.IsNullOrEmpty(reason)) { return GetReason(); }
            return reason;
        }

        private class InputCommand
        {
            public static readonly InputCommand Exit = new InputCommand("exit", "Exit with Reason");
            public static readonly InputCommand SendAndExit = new InputCommand("send", "End of Day --> Send Email and Exit");

            private InputCommand(string value, string description)
            {
                Value = value;
                Description = description;
            }

            public string Value { get; }
            private string Description { get; }

            public override string ToString() => $"{Environment.NewLine}\t[{Value}] = {Description}";
        }
    }
}
