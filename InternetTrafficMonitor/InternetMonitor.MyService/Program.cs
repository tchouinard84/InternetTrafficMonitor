using System.ServiceProcess;

namespace InternetMonitor.MyService
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main()
        {
            var servicesToRun = new ServiceBase[] { new InternetBlockerService() };
            ServiceBase.Run(servicesToRun);
        }
    }
}
