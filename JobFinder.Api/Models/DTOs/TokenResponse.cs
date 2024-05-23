using System.Text.Json.Serialization;

namespace JobFinder.Api.Models.DTOs;

public class TokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    [JsonPropertyName("access_token_expires_in")]
    public int ExpiresIn { get; set; }
}