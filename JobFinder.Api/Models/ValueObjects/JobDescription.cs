using System.Text.Json.Serialization;

namespace JobFinder.Api.Models.ValueObjects;

public record JobDescription
{
    [JsonPropertyName("jobDescriptionId")]
    public string? JobDescriptionId { get; set; }

    [JsonPropertyName("accountingCompany")]
    public string? AccountingCompany { get; set; }

    [JsonPropertyName("jobType")]
    public int? JobType { get; set; }

    [JsonPropertyName("jobNumber")]
    public int? JobNumber { get; set; }

    [JsonPropertyName("paymentClass")]
    public int? PaymentClass { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("shortDescription")]
    public string? ShortDescription { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("functions")]
    public List<string>? Functions { get; set; }

    [JsonPropertyName("skills")]
    public List<string>? Skills { get; set; }

    [JsonPropertyName("offers")]
    public List<string>? Offers { get; set; }

    [JsonPropertyName("functionsTitle")]
    public string? FunctionsTitle { get; set; }

    [JsonPropertyName("skillsTitle")]
    public string? SkillsTitle { get; set; }

    [JsonPropertyName("offersTitle")]
    public string? OffersTitle { get; set; }

    [JsonPropertyName("paymentInfo")]
    public string? PaymentInfo { get; set; }

    [JsonPropertyName("paymentInfoAddition")]
    public string? PaymentInfoAddition { get; set; }
}
