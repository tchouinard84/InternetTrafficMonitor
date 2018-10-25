using InternetMonitor.sender;
using System;
using System.Text;
using System.Threading;
using Console = System.Console;

namespace InternetMonitor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            RunInternetMonitor();
        }

        private static void RunInternetMonitor()
        {
            var monitor = new InternetMonitor();

            var startComment = new StringBuilder();

            var startPrompt = "Did you start the Internet Monitor first thing?";
            var startPromptResponse = GetUserInput(startPrompt + " (yes/no)");

            startComment.AppendLine(startPrompt + " " + startPromptResponse);

            if (startPromptResponse.ToLower().Contains("no"))
            {
                var whyResponse = GetUserInput("Why?");
                startComment.AppendLine($"Why? {whyResponse}");
            }

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                monitor.Start(startComment.ToString());
            }).Start();

            do
            {
                Console.WriteLine($"Commands: {InputCommand.Exit}{InputCommand.SendAndExit}");
                var input = Console.ReadLine();

                if (string.IsNullOrEmpty(input)) { continue; }

                if (input == InputCommand.Exit.Value)
                {
                    monitor.Stop(GetUserInput("Please enter a reason."));
                    break;
                }

                if (input == InputCommand.SendAndExit.Value)
                {
                    monitor.Stop(GetUserInput("Please provide a comment."));
                    SendEmail();
                    break;
                }
            }
            while (true);
        }

        private static string GetUserInput(string message)
        {
            do
            {
                Console.WriteLine(message);
                var userInput = Console.ReadLine();
                if (IsCorrect(userInput)) { return userInput; }
            }
            while (true);
        }

        private static bool IsCorrect(string userInput)
        {
            if (string.IsNullOrEmpty(userInput)) { return false; }

            Console.WriteLine($"You entered: '{userInput}'");
            Console.WriteLine("Is this correct? (y)(n)");

            do
            {
                var correct = Console.ReadLine();
                if (correct == "y") { return true; }
                if (correct == "n") { return false; }

                Console.WriteLine("Incorrect input. Please enter (y) or (n).");
            }
            while (true);
        }

        private static void SendEmail()
        {
            var sender = new InternetHistorySender();
            sender.Send(DateTime.Today);
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
