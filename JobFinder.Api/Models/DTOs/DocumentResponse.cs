using System.Text.Json.Serialization;

namespace JobFinder.Api.Models.DTOs;

public class DocumentResponse
{
    [JsonPropertyName("documentId")]
    public int? DocumentId { get; set; }

    [JsonPropertyName("documentType")]
    public string? DocumentType { get; set; }

    [JsonPropertyName("documentName")]
    public string? DocumentName { get; set; }
}
