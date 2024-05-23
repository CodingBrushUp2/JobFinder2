namespace JobFinder.Api.Models.ValueObjects;

public class JobApplication
{
    public string JobId { get; set; }
    public int DesiredSalary { get; set; }
    public DateTime AvailableFrom { get; set; }
    public bool AgreedToDataProcessing { get; set; }
    public bool AgreedToDataRelaying { get; set; }
    public string? StoreIdList { get; set; }
    public string? RecommendedBy { get; set; }
    public string? ExternalSource { get; set; }
}
