using System.Text.Json.Serialization;

namespace JobFinder.Domain.Models.ValueObjects;

public class Document
{
    [JsonPropertyName("documentType")]
    public string DocumentType { get; set; }

    [JsonPropertyName("documentName")]
    public string DocumentName { get; set; }

    [JsonPropertyName("documentBlob")]
    public string DocumentBlob { get; set; }
}
