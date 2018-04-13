using InternetMonitor.models;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace InternetMonitor.data
{
    public class InternetHistoryData
    {
        private readonly string _filePath;

        public InternetHistoryData()
        {
            _filePath = GetFilePath();
        }

        private static string GetFilePath() => GetFilePath(DateTime.Now);

        private static string GetFilePath(DateTime date)
        {
            var dir = ConfigurationManager.AppSettings["historyDataDirectory"];
            Directory.CreateDirectory(dir);
            var fileNamePostfix = ConfigurationManager.AppSettings["historyDataFilePostfix"];
            return dir + "\\" + date.ToString("yyyy-MM-dd") + fileNamePostfix;
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
            //File.AppendAllText(_filePath, JsonConvert.SerializeObject(entry) + Environment.NewLine);
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
            //return JsonConvert.DeserializeObject<InternetHistoryEntry>(binaryEntry);
        }
    }
}
