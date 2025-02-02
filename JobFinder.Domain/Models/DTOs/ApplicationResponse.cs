﻿using System.Text.Json.Serialization;

namespace JobFinder.Domain.Models.DTOs;

public class ApplicationResponse
{
    [JsonPropertyName("documentId")]
    public int? DocumentId { get; set; }

    [JsonPropertyName("documentType")]
    public string? DocumentType { get; set; }

    [JsonPropertyName("documentName")]
    public string? DocumentName { get; set; }
}
