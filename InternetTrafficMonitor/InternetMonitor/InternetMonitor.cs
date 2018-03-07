using System.Diagnostics;
using System.Threading;

namespace InternetMonitor
{
    public class InternetMonitor
    {
        private readonly InternetLog _internetLog;
        private string _currentTab;
        private string _tab;

        public InternetMonitor()
        {
            _internetLog = new InternetLog();
            _currentTab = "";
            _tab = "";
        }

        public void Run()
        {
            _internetLog.Log("Internet Monitor Started.");

            while (true)
            {
                Thread.Sleep(1000);
                foreach (var process in Process.GetProcessesByName("chrome"))
                {
                    _tab = process.MainWindowTitle;
                    if (_tab == string.Empty) { continue; }
                    if (_tab == _currentTab) { continue; }
                    _currentTab = _tab;
                    _internetLog.MaybeAddAndLog(_currentTab);
                }
            }
        }
    }
}
