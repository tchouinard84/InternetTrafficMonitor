using System.Windows.Automation;

namespace InternetMonitor.Core.browser
{
    public class InternetExplorerUrlRetriever : UrlRetriever
    {
        public override string Browser => "iexplore";

        protected override string GetUrl(AutomationElement element)
        {
            var rebar = element.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, "ReBarWindow32"));
            if (rebar == null)
                return null;

            var edit = rebar.FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));

            return ((ValuePattern)edit.GetCurrentPattern(ValuePattern.Pattern)).Current.Value;
        }
    }
}
