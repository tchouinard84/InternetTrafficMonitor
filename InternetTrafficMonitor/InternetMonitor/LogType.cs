namespace InternetMonitor
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

        private readonly string _value;

        private LogType(string value)
        {
            _value = value;
        }

        public override string ToString()
        {
            return $" [{_value}{DetermineSpaces()}]";
        }

        private string DetermineSpaces()
        {
            if (_value.Length == 5) { return string.Empty; }
            if (_value.Length > 5) { return string.Empty; }
            return " ";
        }
    }
}
