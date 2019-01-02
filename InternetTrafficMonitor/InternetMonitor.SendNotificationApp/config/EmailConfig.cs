using static System.Configuration.ConfigurationManager;

namespace InternetMonitor.SendNotificationApp.config
{
    public class EmailConfig
    {
        public bool IsTest => bool.Parse(AppSettings["IsTest"]);
        public string From => AppSettings["From"];
        public string To => AppSettings["To"];
        public string Cc => AppSettings["Cc"];
        public string Bcc => AppSettings["Bcc"];
        public string Subject => AppSettings["Subject"];
        public string DeveloperEmail => AppSettings["DeveloperEmail"];
    }
}
