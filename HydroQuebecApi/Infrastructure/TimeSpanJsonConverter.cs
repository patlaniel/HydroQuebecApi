using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HydroQuebecApi.Infrastructure
{
    public class TimeSpanJsonConverter : JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
                TimeSpan.ParseExact(reader.GetString()??"00:00:00", @"h\:mm\:ss", CultureInfo.InvariantCulture);

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options) =>
                writer.WriteStringValue(value.ToString(@"h\:mm\:ss", CultureInfo.InvariantCulture));
    }
}
