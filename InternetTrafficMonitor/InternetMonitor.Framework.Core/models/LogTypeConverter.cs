using Newtonsoft.Json;
using System;

namespace InternetMonitor.Framework.Core.models
{
    public sealed class LogTypeConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((LogType)value).Value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return LogType.FromString((string)reader.Value);
        }

        public override bool CanConvert(Type objectType) => objectType == typeof(string);
    }
}
