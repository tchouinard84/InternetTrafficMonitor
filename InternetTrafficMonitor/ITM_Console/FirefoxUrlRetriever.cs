using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace ITM_Console
{
    public class FirefoxUrlRetriever : UrlRetriever
    {
        public FirefoxUrlRetriever()
        {
            TheBrowser = Browser.firefox;
        }

        protected override string GetUrl(AutomationElement element)
        {
            var doc = element.FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Document));
            if (doc == null)
                return null;

            return ((ValuePattern)doc.GetCurrentPattern(ValuePattern.Pattern)).Current.Value as string;
        }
    }
}
