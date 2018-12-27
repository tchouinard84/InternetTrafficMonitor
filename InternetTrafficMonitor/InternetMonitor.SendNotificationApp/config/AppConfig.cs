using System.IO;

namespace InternetMonitor.SendNotificationApp.config
{
    public class AppConfig
    {
        public string BaseDir { get; set; }
        public string DataSubDirectory { get; set; }
        public string TestDataDirectory { get; set; }
        public string DataFilePostfix { get; set; }

        public string GetDataDirectory() => Path.Combine(BaseDir, DataSubDirectory);
        public string GetTestDataDirectory() => Path.Combine(BaseDir, TestDataDirectory);
    }
}
