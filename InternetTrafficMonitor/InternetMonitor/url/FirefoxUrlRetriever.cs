using System.Windows.Automation;

namespace InternetMonitor.url
{
    public class FirefoxUrlRetriever : UrlRetriever
    {
        public override string Browser => "firefox";

        protected override string GetUrl(AutomationElement element)
        {
            var doc = element.FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Document));
            if (doc == null)
                return null;

            return ((ValuePattern)doc.GetCurrentPattern(ValuePattern.Pattern)).Current.Value as string;
        }
    }
}
