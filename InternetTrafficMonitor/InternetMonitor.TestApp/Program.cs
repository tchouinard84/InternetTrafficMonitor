using InternetMonitor.Framework.Core;
using System;
using System.Text;
using System.Timers;
using Timer = System.Timers.Timer;

namespace InternetMonitor.TestApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var startComment = new StringBuilder();

            var startPrompt = "Did you use the Internet before starting the Internet Monitor?";
            var startPromptResponse = GetUserInput(startPrompt + " (yes/no)");

            startComment.AppendLine(startPrompt + " " + startPromptResponse);
            startComment.AppendLine($"Comments: {GetUserInput("Please provide comments.")}");

            var history = new InternetHistory();
            history.Start(startComment.ToString());

            var timer = new Timer { Interval = 1000 };
            timer.Elapsed += OnTimer;
            timer.Start();

            do
            {
                Console.WriteLine("Type 'exit' to exit.");
                var input = Console.ReadLine();

                if (string.IsNullOrEmpty(input)) { continue; }
                if (input != "exit") { continue; }

                history.Stop(GetUserInput("Please enter a reason."));
                break;
            }
            while (true);
        }

        private static void OnTimer(object sender, ElapsedEventArgs e)
        {
            var monitor = new Framework.Core.InternetMonitor();
            monitor.CheckProcesses();
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
    }
}
