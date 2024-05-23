using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace JobFinder.Domain.Models.ValueObjects;

public class Applicant
{
    [JsonPropertyName("titleCode")]
    public string? TitleCode { get; set; }

    [JsonPropertyName("firstName")]
    [Required]
    public string FirstName { get; set; }

    [JsonPropertyName("lastName")]
    [Required]
    public string LastName { get; set; }

    [JsonPropertyName("gender")]
    [Required]
    public string Gender { get; set; }

    [JsonPropertyName("nationality")]
    [Required]
    public string Nationality { get; set; }

    [JsonPropertyName("telephoneNumber")]
    [Required]
    public string TelephoneNumber { get; set; }

    [JsonPropertyName("email")]
    [Required]
    public string Email { get; set; }

    [JsonPropertyName("birthDate")]
    [Required]
    public DateTime BirthDate { get; set; } // Not nullable as it's required

    [JsonPropertyName("countryCode")]
    [Required]
    public string CountryCode { get; set; }

    [JsonPropertyName("zip")]
    [Required]
    public string Zip { get; set; }

    [JsonPropertyName("city")]
    [Required]
    public string City { get; set; }

    [JsonPropertyName("street")]
    [Required]
    public string Street { get; set; }

    [JsonPropertyName("workPermit")]
    [Required]
    public bool WorkPermit { get; set; } // Not nullable as it's required

    [JsonPropertyName("driversLincenseClasses")]
    [Required]
    public string DriversLicenseClasses { get; set; }

    [JsonPropertyName("employedBefore")]
    [Required]
    public bool EmployedBefore { get; set; } // Not nullable as it's required

    [JsonPropertyName("militaryServiceFinished")]
    [Required]
    public bool MilitaryServiceFinished { get; set; } // Not nullable as it's required
}
