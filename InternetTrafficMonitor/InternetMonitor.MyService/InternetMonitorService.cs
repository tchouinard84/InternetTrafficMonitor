using InternetMonitor.Core;
using System.ServiceProcess;
using System.Timers;

namespace InternetMonitor.MyService
{
    public partial class InternetMonitorService : ServiceBase
    {
        private readonly Timer _timer = new Timer();

        public InternetMonitorService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var history = new InternetHistory();
            history.Start("Starting.");

            _timer.Interval = 1000;
            _timer.Elapsed += OnTimer;
            _timer.Start();
        }

        private static void OnTimer(object sender, ElapsedEventArgs e)
        {
            var monitor = new Core.InternetMonitor();
            monitor.CheckProcesses();
        }

        protected override void OnStop()
        {
            var history = new InternetHistory();
            history.Stop("Stopping.");
        }

        protected override void OnPause()
        {
            var history = new InternetHistory();
            history.Stop("Pausing.");
        }

        protected override void OnContinue()
        {
            var history = new InternetHistory();
            history.Start("Continuing.");
        }

        protected override void OnShutdown()
        {
            var history = new InternetHistory();
            history.Stop("Shutting Down.");
        }
    }
}
