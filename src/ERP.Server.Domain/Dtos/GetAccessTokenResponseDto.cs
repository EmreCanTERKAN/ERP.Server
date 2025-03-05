using System.Text.Json.Serialization;

namespace ERP.Server.Domain.Dtos;
public sealed class GetAccessTokenResponseDto
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = default!;
}
