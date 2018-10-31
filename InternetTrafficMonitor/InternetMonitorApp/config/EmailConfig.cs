namespace InternetMonitorApp.config
{
    public class EmailConfig
    {
        public bool IsTest { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Subject { get; set; }
        public string DeveloperEmail { get; set; }
    }
}
