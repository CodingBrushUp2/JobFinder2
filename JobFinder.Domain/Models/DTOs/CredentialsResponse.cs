using System.Text.Json.Serialization;

namespace JobFinder.Domain.Models.DTOs;

public class CredentialsResponse
{
    [JsonPropertyName("id")]
    public int? Id { get; set; }

    [JsonPropertyName("authCode")]
    public string? AuthCode { get; set; }

    [JsonPropertyName("expirationTimestamp")]
    public string? ExpirationTimestamp { get; set; }
}
