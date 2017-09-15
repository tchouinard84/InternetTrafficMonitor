using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace ITM_Console
{
    public class ChromeUrlRetriever : UrlRetriever
    {
        public ChromeUrlRetriever()
        {
            TheBrowser = Browser.chrome;
        }

        protected override string GetUrl(AutomationElement element)
        {
            var edit = element.FindFirst(TreeScope.Subtree,
                new AndCondition(
                    new PropertyCondition(AutomationElement.NameProperty, "address and search bar", PropertyConditionFlags.IgnoreCase),
                    new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit)));
            if (edit == null) { return null; }

            return ((ValuePattern)edit.GetCurrentPattern(ValuePattern.Pattern)).Current.Value as string;
        }
    }
}
