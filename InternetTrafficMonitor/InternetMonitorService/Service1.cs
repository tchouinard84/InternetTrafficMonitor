using System.ServiceProcess;

namespace InternetMonitorService
{
    public partial class Service1 : ServiceBase
    {
        private readonly InternetMonitor.InternetMonitor _monitor;

        public Service1()
        {
            InitializeComponent();
            _monitor = new InternetMonitor.InternetMonitor();
        }

        protected override void OnStart(string[] args)
        {
            _monitor.Start("Internet Monitor Service starting.");
        }

        protected override void OnStop()
        {
            _monitor.Stop("Internet Monitor Service stopping.");
        }
    }
}
