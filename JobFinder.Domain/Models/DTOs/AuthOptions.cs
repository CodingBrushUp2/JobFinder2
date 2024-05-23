namespace JobFinder.Domain.Models.DTOs;

public class AuthOptions
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string TokenUrl { get; set; }

    // Parameterless constructor
    public AuthOptions() { }
}
