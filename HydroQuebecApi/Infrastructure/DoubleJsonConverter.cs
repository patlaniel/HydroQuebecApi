using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HydroQuebecApi.Infrastructure
{
    public class DoubleJsonConverter : JsonConverter<double>
    {
        public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return 0;
            }
            return reader.GetDouble();
        }
        public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options) =>
                writer.WriteStringValue(value.ToString());
    }
}
