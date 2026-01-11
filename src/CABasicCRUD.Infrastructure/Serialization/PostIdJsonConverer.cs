using System.Text.Json;
using System.Text.Json.Serialization;
using CABasicCRUD.Domain.Posts;

namespace CABasicCRUD.Infrastructure.Serialization;

public sealed class PostIdJsonConverter : JsonConverter<PostId>
{
    public override PostId? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        var value = reader.GetGuid();
        return (PostId)value;
    }

    public override void Write(Utf8JsonWriter writer, PostId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value);
    }
}
