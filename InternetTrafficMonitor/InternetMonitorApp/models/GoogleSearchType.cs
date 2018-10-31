using System.Collections.Generic;
using System.Linq;

namespace InternetMonitorApp.models
{
    public sealed class GoogleSearchType
    {
        public static readonly GoogleSearchType Images = new GoogleSearchType("isch", "Images");
        public static readonly GoogleSearchType Videos = new GoogleSearchType("vid", "Videos");
        public static readonly GoogleSearchType Other = new GoogleSearchType(string.Empty, "Other");

        private readonly string _value;
        private readonly string _name;

        private GoogleSearchType(string value, string name)
        {
            _value = value;
            _name = name;
        }

        public static GoogleSearchType FromString(string value)
        {
            if (value == null) { return Other; }
            return Values().FirstOrDefault(t => t._value == value);
        }

        private static IEnumerable<GoogleSearchType> Values() => new[] { Images, Videos, Other };

        public override string ToString() => _name;
    }
}
