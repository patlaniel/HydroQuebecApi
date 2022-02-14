using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HydroQuebecApi.Infrastructure
{
    public class Int32JsonConverter : JsonConverter<Int32>
    {
        public override Int32 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
                (Int32)reader.GetDouble();

        public override void Write(Utf8JsonWriter writer, Int32 value, JsonSerializerOptions options) =>
                writer.WriteStringValue(value.ToString());
    }
}
