using InternetMonitorApp.config;
using InternetMonitorApp.models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace InternetMonitorApp.data
{
    public class InternetHistoryData : IInternetHistoryData
    {
        private readonly AppConfig _config;
        private readonly string _filePath;

        public InternetHistoryData(IOptions<AppConfig> configOptions)
        {
            _config = configOptions.Value;
            _filePath = GetFilePath();
        }

        private string GetFilePath() => GetFilePath(DateTime.Now);

        private string GetFilePath(DateTime date)
        {
            var dir = _config.GetDataDirectory();
            Directory.CreateDirectory(dir);
            return dir + "\\" + date.ToString("yyyy-MM-dd") + _config.DataFilePostfix;
        }

        public void Write(InternetHistoryEntry entry)
        {
            var ms = new MemoryStream();
            using (var writer = new BsonDataWriter(ms))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(writer, entry);
            }

            File.AppendAllText(_filePath, Convert.ToBase64String(ms.ToArray()) + Environment.NewLine);
        }

        public IReadOnlyCollection<InternetHistoryEntry> Read()
        {
            if (!File.Exists(_filePath)) { return new List<InternetHistoryEntry>(); }

            var binaryData = File.ReadAllLines(_filePath);
            return binaryData.Select(Deserialize).ToList();
        }

        public IReadOnlyCollection<InternetHistoryEntry> Read(DateTime date)
        {
            var filePath = GetFilePath(date);
            if (!File.Exists(filePath)) { return new List<InternetHistoryEntry>(); }

            var binaryData = File.ReadAllLines(filePath);
            return binaryData.Select(Deserialize).ToList();
        }

        private static InternetHistoryEntry Deserialize(string binaryEntry)
        {
            var json = Convert.FromBase64String(binaryEntry);
            var ms = new MemoryStream(json);

            using (var reader = new BsonDataReader(ms))
            {
                return new JsonSerializer().Deserialize<InternetHistoryEntry>(reader);
            }
        }
    }
}
