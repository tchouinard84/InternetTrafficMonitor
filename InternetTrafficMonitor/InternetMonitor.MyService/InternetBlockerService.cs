using NLog;
using System.ServiceProcess;
using System.Timers;

namespace InternetMonitor.MyService
{
    public partial class InternetBlockerService : ServiceBase
    {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();
        private Timer _timer;

        public InternetBlockerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            log.Info("Starting Service");

            _timer = new Timer { Interval = 1000 };
            _timer.Elapsed += OnTimer;
            _timer.Start();
        }

        private static void OnTimer(object sender, ElapsedEventArgs e)
        {
            var monitor = new Framework.Core.InternetMonitor();
            monitor.CheckProcesses();
        }

        protected override void OnStop()
        {
            log.Info("Stopping Service");
        }

        protected override void OnPause()
        {
            log.Info("Pausing Service");
        }

        protected override void OnContinue()
        {
            log.Info("Continue Service");
        }

        protected override void OnShutdown()
        {
            log.Info("Shutdown Service");
        }
    }
}
