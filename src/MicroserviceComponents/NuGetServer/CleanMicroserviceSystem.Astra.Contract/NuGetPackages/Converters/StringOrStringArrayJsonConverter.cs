using System.Text.Json;
using System.Text.Json.Serialization;

namespace CleanMicroserviceSystem.Astra.Contract.NuGetPackages.Converters;

public class StringOrStringArrayJsonConverter : JsonConverter<IReadOnlyList<string>>
{
    public override IReadOnlyList<string> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
            return new List<string> { reader.GetString() };

        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException();

        var result = new List<string>();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.String)
                result.Add(reader.GetString());
            else if (reader.TokenType == JsonTokenType.EndArray)
            {
                return result;
            }
            else
            {
                break;
            }
        }

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, IReadOnlyList<string> values, JsonSerializerOptions options)
    {
        writer.WriteStartArray();

        foreach (var value in values)
        {
            writer.WriteStringValue(value);
        }

        writer.WriteEndArray();
    }
}