using System.Text.Json.Serialization;

namespace JobFinder.Domain.Models.ValueObjects
{
    public record Job
    {
        [JsonPropertyName("jobId")]
        public string? JobId { get; set; }

        [JsonPropertyName("jobDescriptionId")]
        public string? JobDescriptionId { get; set; }

        [JsonPropertyName("accountingCompanyId")]
        public string? AccountingCompanyId { get; set; }

        [JsonPropertyName("accountingCompany")]
        public string? AccountingCompany { get; set; }

        [JsonPropertyName("displayAccountingCompanyId")]
        public string? DisplayAccountingCompanyId { get; set; }

        [JsonPropertyName("displayAccountingCompany")]
        public string? DisplayAccountingCompany { get; set; }

        [JsonPropertyName("jobType")]
        public string? JobType { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("shortDescription")]
        public string? ShortDescription { get; set; }

        [JsonPropertyName("employmentLevelId")]
        public string? EmploymentLevelId { get; set; }

        [JsonPropertyName("employmentLevel")]
        public string? EmploymentLevel { get; set; }

        [JsonPropertyName("jobNumber")]
        public int? JobNumber { get; set; }

        [JsonPropertyName("paymentClass")]
        public int? PaymentClass { get; set; }

        [JsonPropertyName("jobGroups")]
        public List<JobGroup>? JobGroups { get; set; }

        [JsonPropertyName("countryCode")]
        public string? CountryCode { get; set; }

        [JsonPropertyName("zip")]
        public string? Zip { get; set; }

        [JsonPropertyName("city")]
        public string? City { get; set; }

        [JsonPropertyName("provinceId")]
        public string? ProvinceId { get; set; }

        [JsonPropertyName("provinceNumber")]
        public int? ProvinceNumber { get; set; }

        [JsonPropertyName("provinceName")]
        public string? ProvinceName { get; set; }

        [JsonPropertyName("districtId")]
        public string? DistrictId { get; set; }

        [JsonPropertyName("districtNumber")]
        public int? DistrictNumber { get; set; }

        [JsonPropertyName("districtName")]
        public string? DistrictName { get; set; }

        [JsonPropertyName("startDate")]
        public DateTime? StartDate { get; set; }

        [JsonPropertyName("creationDate")]
        public DateTime? CreationDate { get; set; }

        [JsonPropertyName("hours")]
        public double? Hours { get; set; }

        [JsonPropertyName("minHours")]
        public double? MinHours { get; set; }

        [JsonPropertyName("maxHours")]
        public double? MaxHours { get; set; } // Nullable in case it's not present

        [JsonPropertyName("amountOfJobs")]
        public int? AmountOfJobs { get; set; }

        [JsonPropertyName("stores")]
        public List<Store>? Stores { get; set; }

        [JsonPropertyName("links")]
        public List<Link>? Links { get; set; }

        [JsonPropertyName("synonyms")]
        public List<string>? Synonyms { get; set; }

        [JsonPropertyName("jobLevels")]
        public List<string>? JobLevels { get; set; }
    }

    public record JobGroup
    {
        [JsonPropertyName("jobGroupId")]
        public int? JobGroupId { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("subGroups")]
        public List<JobSubGroup>? SubGroups { get; set; }
    }

    public record JobSubGroup
    {
        [JsonPropertyName("jobGroupId")]
        public int? JobGroupId { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }

    public record Store
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("street")]
        public string? Street { get; set; }

        [JsonPropertyName("amountOfJobs")]
        public int? AmountOfJobs { get; set; }
    }

    public record Link
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("method")]
        public string? Method { get; set; }

        [JsonPropertyName("href")]
        public string? Href { get; set; }
    }
}
