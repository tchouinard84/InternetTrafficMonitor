using System.IO;
using static System.Configuration.ConfigurationManager;

namespace InternetMonitor.Framework.Core.Config
{
    public class AppConfig
    {
        public bool IsTest => bool.Parse(AppSettings["IsTest"]);
        public string BaseDir => AppSettings["BaseDir"];
        public string DataDir => AppSettings["DataDir"];
        public string TestDataDir => AppSettings["TestDataDir"];
        public string DataFilePostfix => AppSettings["DataFilePostfix"];
        public string InputFilesDir => AppSettings["InputFilesDir"];
        public string AlertItemsFileName => AppSettings["AlertItemsFileName"];
        public string IgnoreItemsFileName => AppSettings["IgnoreItemsFileName"];

        public string GetDataDirectory() => Path.Combine(BaseDir, DataDir);
        public string GetTestDataDirectory() => Path.Combine(BaseDir, TestDataDir);
        public string GetAlertItemsFilePath() => Path.Combine(BaseDir, InputFilesDir, AlertItemsFileName);
        public string GetIgnoreItemsFilePath() => Path.Combine(BaseDir, InputFilesDir, IgnoreItemsFileName);
    }
}
