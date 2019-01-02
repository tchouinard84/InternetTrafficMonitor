using System.Collections.Generic;
using System.Linq;

namespace InternetMonitor.Framework.Core.models
{
    public sealed class YouTubeType
    {
        public static readonly YouTubeType Video = new YouTubeType("v", "Video");
        public static readonly YouTubeType Search = new YouTubeType("search_query", "Search");
        public static readonly YouTubeType Playlist = new YouTubeType("list", "Playlist");
        public static readonly YouTubeType Other = new YouTubeType(string.Empty, "Other");
        public static readonly YouTubeType Unknown = new YouTubeType(null, "Unknown");

        private readonly string _value;
        private readonly string _name;

        private YouTubeType(string value, string name)
        {
            _value = value;
            _name = name;
        }

        public string Value => _value;

        public static YouTubeType FromString(string value)
        {
            if (value == null) { return Other; }
            var type = Values().FirstOrDefault(t => t._value == value);
            if (type == null) { return Unknown; }
            return type;
        }

        private static IEnumerable<YouTubeType> Values() => new[] { Video, Search, Playlist, Other, Unknown };

        public override string ToString() => _name;
    }
}
