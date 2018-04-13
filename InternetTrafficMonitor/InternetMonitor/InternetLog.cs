using InternetMonitor.models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace InternetMonitor
{
    public class InternetLog
    {
        private const string BASE_DIR = @"C:\Users\tchouina\Personal\InternetMonitor";
        private const string LOG_DIR = BASE_DIR + @"\logs";
        private const string ALERT_ITEMS_FILE_PATH = BASE_DIR + @"\alert_words.txt";
        private const string IGNORE_ITEMS_FILE_PATH = BASE_DIR + @"\ignore_items.txt";

        private readonly List<string> _websites;
        private List<string> _alertWords;
        private List<string> _ignoreItems;
        private readonly string _filePath;

        public InternetLog()
        {
            if (LoggingOff()) { return; }
            Directory.CreateDirectory(LOG_DIR);
            _filePath = LOG_DIR + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + "_internet_monitor.log";
            _websites = new List<string>();
            InitializeAlertWords();
            InitializeIgnoreItems();
        }

        private void InitializeAlertWords()
        {
            _alertWords = new List<string>();
            try
            {
                var alertContent = File.ReadAllLines(ALERT_ITEMS_FILE_PATH);
                foreach (var word in alertContent) { _alertWords.Add(word); }
            }
            catch (Exception e)
            {
                Log(LogType.Error, $"Error trying to initialize alert words from file: {ALERT_ITEMS_FILE_PATH}");
            }
        }

        private void InitializeIgnoreItems()
        {
            _ignoreItems = new List<string>();
            try
            {
                var ignoreItems = File.ReadAllLines(IGNORE_ITEMS_FILE_PATH);
                foreach (var item in ignoreItems) { _ignoreItems.Add(item); }
            }
            catch (Exception e)
            {
                Log(LogType.Error, $"Error trying to initialize items to ignore from file: {IGNORE_ITEMS_FILE_PATH}");
            }
        }

        public void Log(string message)
        {
            if (LoggingOff()) { return; }
            File.AppendAllText(_filePath, $"{DateTime.Now}{DetermineType(message)} : {message}{Environment.NewLine}");
        }

        public void Log(LogType logtype, string message)
        {
            if (LoggingOff()) { return; }
            File.AppendAllText(_filePath, $"{DateTime.Now}{logtype} : {message}{Environment.NewLine}");
        }

        private string DetermineType(string message)
        {
            foreach (var word in _alertWords)
            {
                if (message.ToLower().Contains(word)) { return LogType.Alert.ToString(); }
            }

            return LogType.Info.ToString();
        }

        public void MaybeAddAndLog(string website)
        {
            if (LoggingOff()) { return; }
            if (_websites.Contains(website)) { return; }

            foreach (var item in _ignoreItems)
            {
                if (website.Contains(item)) { return; }
            }

            Log(website);
            _websites.Add(website);
        }

        public bool LoggingOff() => bool.Parse(ConfigurationManager.AppSettings["loggingOff"]);
    }
}
