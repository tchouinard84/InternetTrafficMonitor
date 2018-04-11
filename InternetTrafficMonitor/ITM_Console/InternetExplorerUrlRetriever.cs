using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace ITM_Console
{
    public class InternetExplorerUrlRetriever : UrlRetriever
    {
        public InternetExplorerUrlRetriever()
        {
            TheBrowser = Browser.iexplore;
        }

        protected override string GetUrl(AutomationElement element)
        {
            var rebar = element.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, "ReBarWindow32"));
            if (rebar == null)
                return null;

            var edit = rebar.FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));

            return ((ValuePattern)edit.GetCurrentPattern(ValuePattern.Pattern)).Current.Value as string;
        }
    }
}
