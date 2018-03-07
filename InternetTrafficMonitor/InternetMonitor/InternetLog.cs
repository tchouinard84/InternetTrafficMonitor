using System;
using System.Collections.Generic;
using System.IO;

namespace InternetMonitor
{
    public class InternetLog
    {
        private const string BASE_DIR = @"C:\Users\tchouina\Personal\InternetMonitor";
        private const string LOG_DIR = BASE_DIR + @"\logs";
        private const string ALERT_CONTENT_FILE_PATH = BASE_DIR + @"\inappropriate_content.txt";

        private const string ALERT = " [ALERT]";
        private const string INFO  = " [INFO ]";

        private readonly List<string> _websites;
        private List<string> _alertWords;
        private readonly string _filePath;

        public InternetLog()
        {
            Directory.CreateDirectory(LOG_DIR);
            _filePath = LOG_DIR + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + "_internet_monitor.log";
            _websites = new List<string>();
            InitializeAlertWords();
        }

        private void InitializeAlertWords()
        {
            var alertContent = File.ReadLines(ALERT_CONTENT_FILE_PATH);
            _alertWords = new List<string>();
            foreach (var word in alertContent) { _alertWords.Add(word); }
        }

        public void Log(string message)
        {
            File.AppendAllText(_filePath, $"{DateTime.Now}{DetermineType(message)} : {message}{Environment.NewLine}");
        }

        private string DetermineType(string message)
        {
            foreach (var word in _alertWords)
            {
                if (message.ToLower().Contains(word)) { return ALERT; }
            }

            return INFO;
        }

        public void MaybeAddAndLog(string website)
        {
            if (_websites.Contains(website)) { return; }
            Log(website);
            _websites.Add(website);
        }
    }
}
