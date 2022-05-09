using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.Utils
{
    public class TimeOnlySerializer : JsonConverter<TimeOnly>
    {
        private readonly string serializationFormat;

        public TimeOnlySerializer() : this(null)
        {
        }

        public TimeOnlySerializer(string? serializationFormat)
        {
            this.serializationFormat = serializationFormat ?? "HH:mm:ss.fff";
        }

        public override TimeOnly Read(ref Utf8JsonReader reader,
                                Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return TimeOnly.Parse(value!);
        }

        public override void Write(Utf8JsonWriter writer, TimeOnly value,
                                            JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString(serializationFormat));

    }
}
