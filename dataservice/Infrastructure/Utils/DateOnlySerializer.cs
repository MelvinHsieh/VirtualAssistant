using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.Utils
{
    public class DateOnlySerializer : JsonConverter<DateOnly>
    {
        private readonly string serializationFormat;

        public DateOnlySerializer() : this(null)
        {
        }

        public DateOnlySerializer(string? serializationFormat)
        {
            this.serializationFormat = serializationFormat ?? "dd/MM/yyyy";
        }

        public override DateOnly Read(ref Utf8JsonReader reader,
                                Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return DateOnly.Parse(value!);
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value,
                                            JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString(serializationFormat));

    }
}
