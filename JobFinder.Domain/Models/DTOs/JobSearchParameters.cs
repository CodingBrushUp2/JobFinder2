namespace JobFinder.Domain.Models.DTOs;

public class JobSearchParameters
{
    //public string? JobGroupIds { get; set; }
    //public string? SubJobGroupIds { get; set; }
    //public string? JobTypeIds { get; set; }
    //public string? ProvinceIdList { get; set; }
    //public string? DistrictIdList { get; set; }
    public EmploymentLevel? EmploymentLevelId { get; set; } = EmploymentLevel.T;
    public string? SearchTerm { get; set; }
    //public string? JobLevels { get; set; }
    //public string? JobDescriptionId { get; set; }
    //public string? CityList { get; set; }
    //public string? Zip { get; set; }
    //public double? MinWorkingHours { get; set; }
    //public double? MaxWorkingHours { get; set; }
    public int? Offset { get; set; } = 0;
    public int? Limit { get; set; } = 10;
    public string? SortField { get; set; }
    public string? SortDirection { get; set; }
    //public bool? IncludeInternal { get; set; }
}
