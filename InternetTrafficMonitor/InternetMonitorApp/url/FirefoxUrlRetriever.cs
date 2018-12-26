using System.Windows.Automation;

namespace InternetMonitorApp.url
{
    public class FirefoxUrlRetriever : UrlRetriever
    {
        protected override string GetUrl(AutomationElement element)
        {
            var doc = element.FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Document));
            if (doc == null)
                return null;

            return ((ValuePattern)doc.GetCurrentPattern(ValuePattern.Pattern)).Current.Value as string;
        }
    }
}
