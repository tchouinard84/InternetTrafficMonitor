namespace InternetMonitor.Core.config
{
    public interface IAppConfig
    {
        bool IsTest { get; set; }
        string BaseDir { get; set; }
        string DataDir { get; set; }
        string TestDataDir { get; set; }
        string DataFilePostfix { get; set; }
        string InputFilesDir { get; set; }
        string AlertItemsFileName { get; set; }
        string IgnoreItemsFileName { get; set; }

        string GetDataDirectory();
        string GetTestDataDirectory();
        string GetAlertItemsFilePath();
        string GetIgnoreItemsFilePath();
    }
}