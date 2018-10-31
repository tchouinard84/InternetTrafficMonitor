using System.IO;

namespace InternetMonitorApp.config
{
    public class AppConfig
    {
        public string DataSubDirectory { get; set; }
        public string DataFilePostfix { get; set; }
        public string InputFilesSubDirectory { get; set; }
        public string AlertItemsFileName { get; set; }
        public string IgnoreItemsFileName { get; set; }

        public string GetDataDirectory() => Path.Combine(GetBaseDir(), DataSubDirectory);
        public string GetAlertItemsFilePath() => Path.Combine(GetBaseDir(), InputFilesSubDirectory, AlertItemsFileName);
        public string GetIgnoreItemsFilePath() => Path.Combine(GetBaseDir(), InputFilesSubDirectory, IgnoreItemsFileName);

        public string GetBaseDir() => $"{Directory.GetCurrentDirectory()}\\..";
    }
}
