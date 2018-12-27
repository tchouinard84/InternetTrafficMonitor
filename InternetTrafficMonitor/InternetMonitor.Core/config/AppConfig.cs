using System.IO;
using static System.Configuration.ConfigurationManager;

namespace InternetMonitor.Core.config
{
    public class AppConfig : IAppConfig
    {
        private bool? _isTest;
        private string _baseDir;
        private string _dataDir;
        private string _testDataDir;
        private string _dataFilePostfix;
        private string _inputFilesDir;
        private string _alertItemsFileName;
        private string _ignoreItemsFileName;

        public bool IsTest
        {
            get
            {
                if (_isTest.HasValue) { return _isTest.Value; }
                return bool.Parse(AppSettings["IsTest"]);
            }
            set => _isTest = value;
        }

        public string BaseDir
        {
            get
            {
                if (!string.IsNullOrEmpty(_baseDir)) { return _baseDir; }
                return AppSettings["BaseDir"];
            }
            set => _baseDir = value;
        }

        public string DataDir {
            get {
                if (!string.IsNullOrEmpty(_dataDir)) { return _dataDir; }
                return AppSettings["DataDir"];
            }
            set => _dataDir = value;
        }

        public string TestDataDir {
            get {
                if (!string.IsNullOrEmpty(_testDataDir)) { return _testDataDir; }
                return AppSettings["TestDataDir"];
            }
            set => _testDataDir = value;
        }

        public string DataFilePostfix {
            get {
                if (!string.IsNullOrEmpty(_dataFilePostfix)) { return _dataFilePostfix; }
                return AppSettings["DataFilePostfix"];
            }
            set => _dataFilePostfix = value;
        }

        public string InputFilesDir {
            get {
                if (!string.IsNullOrEmpty(_inputFilesDir)) { return _inputFilesDir; }
                return AppSettings["InputFilesDir"];
            }
            set => _inputFilesDir = value;
        }

        public string AlertItemsFileName {
            get {
                if (!string.IsNullOrEmpty(_alertItemsFileName)) { return _alertItemsFileName; }
                return AppSettings["AlertItemsFileName"];
            }
            set => _alertItemsFileName = value;
        }

        public string IgnoreItemsFileName {
            get {
                if (!string.IsNullOrEmpty(_ignoreItemsFileName)) { return _ignoreItemsFileName; }
                return AppSettings["IgnoreItemsFileName"];
            }
            set => _ignoreItemsFileName = value;
        }

        public string GetDataDirectory() => Path.Combine(BaseDir, DataDir);
        public string GetTestDataDirectory() => Path.Combine(BaseDir, TestDataDir);
        public string GetAlertItemsFilePath() => Path.Combine(BaseDir, InputFilesDir, AlertItemsFileName);
        public string GetIgnoreItemsFilePath() => Path.Combine(BaseDir, InputFilesDir, IgnoreItemsFileName);
    }
}
