using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Tollminder.Core.Helpers
{
    public class DateTimeFromUnixConverterHelper : DateTimeConverterBase
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue(DateHelper.UnixTime((DateTime)value).ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) { return null; }
            return DateHelper.UnixTimeStampToDateTime((long)reader.Value);
        }

    }
}
