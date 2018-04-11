using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetMonitor
{
    public class GoogleSearch : WebUrl
    {
        public GoogleSearchType Type { get; set; }
        public string SearchQuery { get; set; }
        public bool IsSafeSearch { get; set; }
    }
}
