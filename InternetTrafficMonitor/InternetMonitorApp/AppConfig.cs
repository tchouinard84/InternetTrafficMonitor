namespace InternetMonitorApp
{
    public class AppConfig
    {
        public bool IsTest { get; set; }
        public string EmailTo { get; set; }
        public string EmailCc { get; set; }
        public string EmailBcc { get; set; }
        public string DeveloperEmail { get; set; }
        public string DataDirectory { get; set; }
        public string DataFilePostfix { get; set; }
    }
}
