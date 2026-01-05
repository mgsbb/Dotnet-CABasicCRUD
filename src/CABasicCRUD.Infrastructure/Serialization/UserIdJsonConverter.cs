using System.Text.Json;
using System.Text.Json.Serialization;
using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Infrastructure.Serialization;

public sealed class UserIdJsonConverter : JsonConverter<UserId>
{
    public override UserId? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        var value = reader.GetGuid();
        return (UserId)value;
    }

    public override void Write(Utf8JsonWriter writer, UserId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value);
    }
}
