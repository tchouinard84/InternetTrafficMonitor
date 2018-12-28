using System.Collections.Generic;
using System.Linq;

namespace InternetMonitor.Framework.Core.models
{
    public sealed class LogType
    {
        public static readonly LogType Info = new LogType("INFO");
        public static readonly LogType Alert = new LogType("ALERT");
        public static readonly LogType Error = new LogType("ERROR");
        public static readonly LogType Start = new LogType("START");
        public static readonly LogType Stop = new LogType("STOP");
        public static readonly LogType Pause = new LogType("PAUSE");
        public static readonly LogType Continue = new LogType("CONTINUE");

        public string Value { get; }

        private LogType(string value)
        {
            Value = value;
        }

        private static IEnumerable<LogType> Values()
        {
            return new [] { Info, Alert, Error, Start, Stop, Pause, Continue };
        }

        public override string ToString()
        {
            return $" [{Value}{DetermineSpaces()}]";
        }

        private string DetermineSpaces()
        {
            if (Value.Length == 5) { return string.Empty; }
            if (Value.Length > 5) { return string.Empty; }
            return " ";
        }

        public static LogType FromString(string value)
        {
            return Values().FirstOrDefault(lt => lt.Value == value);
        }
    }
}
