using JobFinder.Domain.Models.Contracts;
using JobFinder.Domain.Models.DTOs;
using JobFinder.Domain.Models.ValueObjects;
using JobFinder.Domain.Services;
using JobFinder.Domain.Utilities;
using RestSharp;
using System.Text.Json;

namespace JobFinder.Api.Services;

public class JobService : IJobService
{
    private readonly RestClient _client;
    private readonly IAuthService _authService;
    private readonly ILogger<JobService> _logger;

    public JobService(IAuthService authService, ILogger<JobService> logger)
    {
        _client = new RestClient("https://dev.apply.xxxxx/api");
        _authService = authService;
        _logger = logger;
    }

    public async Task<Result<SearchJobsResponse>> SearchJobsAsync(JobSearchParameters parameters)
    {
        var token = await _authService.GetAccessTokenAsync();
        var request = BuildSearchJobsRequest(parameters, token);

        var response = await _client.ExecuteAsync(request);
        if (!response.IsSuccessful)
        {
            _logger.LogError("Failed to search jobs. Status Code: {StatusCode}, Error Message: {ErrorMessage}", response.StatusCode, response.ErrorMessage);
            return Result<SearchJobsResponse>.Failure("Failed to search jobs", (int)response.StatusCode);
        }

        var searchJobsResponse = DeserializeResponse<SearchJobsResponse>(response.Content);
        return Result<SearchJobsResponse>.Success(searchJobsResponse);
    }

    public async Task<Result<JobDescription>> GetJobDescriptionAsync(string jobDescriptionId)
    {
        var token = await _authService.GetAccessTokenAsync();
        var request = new RestRequest($"jobs/{jobDescriptionId}", Method.Get);
        request.AddHeader("Authorization", $"Bearer {token}");

        var response = await _client.ExecuteAsync(request);
        if (!response.IsSuccessful)
        {
            _logger.LogError("Failed to get job description. Status Code: {StatusCode}, Error Message: {ErrorMessage}", response.StatusCode, response.ErrorMessage);
            return Result<JobDescription>.Failure("Failed to get job description", (int)response.StatusCode);
        }

        var jobDescription = DeserializeResponse<JobDescription>(response.Content);
        return Result<JobDescription>.Success(jobDescription);
    }

    public async Task<Result<CredentialsResponse>> SelectJobAsync(string challengeType, string challengeKey, string? challengeAnswer, string? correlationId)
    {
        var token = await _authService.GetAccessTokenAsync();
        var request = new RestRequest("job-applications/credentials", Method.Post);
        request.AddHeader("Authorization", $"Bearer {token}");
        request.AddHeader("ChallengeType", challengeType);
        request.AddHeader("ChallengeKey", challengeKey);
        //request.AddHeader("ChallengeAnswer", challengeAnswer);
        //request.AddHeader("correlation-id", correlationId);
        request.AddHeader("Accept", "application/json");

        var response = await _client.ExecuteAsync(request);
        if (!response.IsSuccessful)
        {
            _logger.LogError("Failed to select job. Status Code: {StatusCode}, Error Message: {ErrorMessage}", response.StatusCode, response.ErrorMessage);
            return Result<CredentialsResponse>.Failure("Failed to select job", (int)response.StatusCode);
        }

        var credentialsResponse = JsonSerializer.Deserialize<CredentialsResponse>(response.Content);
        return Result<CredentialsResponse>.Success(credentialsResponse, (int)response.StatusCode);
    }
    public async Task<Result> SubmitApplicantAsync(string applicationId, Applicant applicant, string authCode, string correlationId)
    {
        var token = await _authService.GetAccessTokenAsync();
        var request = new RestRequest($"job-applications/{applicationId}/applicant", Method.Put);
        request.AddHeader("Authorization", $"Bearer {token}");
        request.AddHeader("application-auth-code", authCode);
        request.AddHeader("correlation-id", correlationId);
        request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

        // Add form data
        request.AddParameter("titleCode", applicant.TitleCode);
        request.AddParameter("firstName", applicant.FirstName);
        request.AddParameter("lastName", applicant.LastName);
        request.AddParameter("gender", applicant.Gender);
        request.AddParameter("nationality", applicant.Nationality);
        request.AddParameter("telephoneNumber", applicant.TelephoneNumber);
        request.AddParameter("email", applicant.Email);
        request.AddParameter("birthDate", applicant.BirthDate.ToString("yyyy-MM-dd"));
        request.AddParameter("countryCode", applicant.CountryCode);
        request.AddParameter("zip", applicant.Zip);
        request.AddParameter("city", applicant.City);
        request.AddParameter("street", applicant.Street);
        request.AddParameter("workPermit", applicant.WorkPermit.ToString().ToLower());
        request.AddParameter("driversLincenseClasses", applicant.DriversLicenseClasses);
        request.AddParameter("employedBefore", applicant.EmployedBefore.ToString().ToLower());
        request.AddParameter("militaryServiceFinished", applicant.MilitaryServiceFinished.ToString().ToLower());

        var response = await _client.ExecuteAsync(request);
        if (!response.IsSuccessful)
        {
            _logger.LogError("Failed to submit applicant. Status Code: {StatusCode}, Error Message: {ErrorMessage}", response.StatusCode, response.ErrorMessage);
            return Result.Failure(response.ErrorMessage, (int)response.StatusCode);
        }

        return Result.Success((int)response.StatusCode);
    }

    public async Task<Result<DocumentResponse>> UploadDocumentAsync(string applicationId, Document document, string authCode, string correlationId)
    {
        var token = await _authService.GetAccessTokenAsync();
        var request = new RestRequest($"job-applications/{applicationId}/documents", Method.Post);
        request.AddHeader("Authorization", $"Bearer {token}");
        request.AddHeader("application-auth-code", authCode);
        request.AddHeader("correlation-id", correlationId);
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Accept", "application/json");

        request.AddJsonBody(document);

        var response = await _client.ExecuteAsync(request);
        if (!response.IsSuccessful)
        {
            _logger.LogError("Failed to upload document. Status Code: {StatusCode}, Error Message: {ErrorMessage}", response.StatusCode, response.ErrorMessage);
            return Result<DocumentResponse>.Failure("Failed to upload document", (int)response.StatusCode);
        }

        var documentResponse = JsonSerializer.Deserialize<DocumentResponse>(response.Content);
        return Result<DocumentResponse>.Success(documentResponse, (int)response.StatusCode);
    }

    public async Task<Result<ApplicationResponse>> SubmitApplicationAsync(string applicationId, JobApplication jobApplication, string authCode, string correlationId)
    {
        var token = await _authService.GetAccessTokenAsync();
        var request = new RestRequest($"job-applications/{applicationId}/submit", Method.Post);
        request.AddHeader("Authorization", $"Bearer {token}");
        request.AddHeader("application-auth-code", authCode);
        request.AddHeader("correlation-id", correlationId);
        request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
        request.AddHeader("Accept", "application/json");

        request.AddParameter("jobId", jobApplication.JobId);
        request.AddParameter("desiredSalary", jobApplication.DesiredSalary);
        request.AddParameter("availableFrom", jobApplication.AvailableFrom.ToString("MM-dd-yyyy"));
        request.AddParameter("agreedToDataProcessing", jobApplication.AgreedToDataProcessing);
        request.AddParameter("agreedToDataRelaying", jobApplication.AgreedToDataRelaying);
        request.AddParameter("storeIdList", jobApplication.StoreIdList);
        request.AddParameter("recommendedBy", jobApplication.RecommendedBy);
        request.AddParameter("externalSource", jobApplication.ExternalSource);

        var response = await _client.ExecuteAsync(request);
        if (!response.IsSuccessful)
        {
            _logger.LogError("Failed to submit application. Status Code: {StatusCode}, Error Message: {ErrorMessage}", response.StatusCode, response.ErrorMessage);
            return Result<ApplicationResponse>.Failure("Failed to submit application", (int)response.StatusCode);
        }

        var application = JsonSerializer.Deserialize<ApplicationResponse>(response.Content);
        return Result<ApplicationResponse>.Success(application, (int)response.StatusCode);
    }

    public async Task<Result<ApplicationResponse>> GetApplicationAsync(string applicationId, string authCode, string correlationId)
    {
        var token = await _authService.GetAccessTokenAsync();
        var request = new RestRequest($"job-applications/{applicationId}/submit", Method.Get);
        request.AddHeader("Authorization", $"Bearer {token}");
        request.AddHeader("application-auth-code", authCode);
        request.AddHeader("correlation-id", correlationId);
        request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
        request.AddHeader("Accept", "application/json");

        //request.AddParameter("applicationId", applicationId);

        var response = await _client.ExecuteAsync(request);
        if (!response.IsSuccessful)
        {
            _logger.LogError("Failed to submit application. Status Code: {StatusCode}, Error Message: {ErrorMessage}", response.StatusCode, response.ErrorMessage);
            return Result<ApplicationResponse>.Failure("Failed to submit application", (int)response.StatusCode);
        }

        var application = JsonSerializer.Deserialize<ApplicationResponse>(response.Content);
        return Result<ApplicationResponse>.Success(application, (int)response.StatusCode);
    }

    private T DeserializeResponse<T>(string responseContent)
    {
        try
        {
            return JsonSerializer.Deserialize<T>(responseContent);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to deserialize response");
            throw new Exception("Failed to deserialize response", ex);
        }
    }
    private static RestRequest? BuildSearchJobsRequest(JobSearchParameters parameters, string token)
    {
        var request = new RestRequest("jobs/search", Method.Post);
        request.AddHeader("Authorization", $"Bearer {token}");
        request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

        if (parameters != null)
        {
            //request.AddParameter("jobGroupIds", parameters.JobGroupIds);
            //request.AddParameter("subJobGroupIds", parameters.SubJobGroupIds);
            //request.AddParameter("jobTypeIds", parameters.JobTypeIds);
            //request.AddParameter("provinceIdList", parameters.ProvinceIdList);
            //request.AddParameter("districtIdList", parameters.DistrictIdList);
            request.AddParameter("employmentLevelId", parameters.EmploymentLevelId ?? EmploymentLevel.T);
            request.AddParameter("searchTerm", parameters.SearchTerm ?? string.Empty);
            //request.AddParameter("jobLevels", parameters.JobLevels);
            //request.AddParameter("jobDescriptionId", parameters.JobDescriptionId);
            //request.AddParameter("cityList", parameters.CityList);
            //request.AddParameter("zip", parameters.Zip);
            //request.AddParameter("minWorkingHours", parameters.MinWorkingHours);
            //request.AddParameter("maxWorkingHours", parameters.MaxWorkingHours);
            request.AddParameter("offset", parameters.Offset ?? 0);
            request.AddParameter("limit", parameters.Limit ?? 10);
            request.AddParameter("sortField", parameters.SortField ?? "EmploymentLevel");
            request.AddParameter("sortDirection", parameters.SortDirection ?? "Ascending");
            //request.AddParameter("includeInternal", parameters.IncludeInternal);
        }

        return request;
    }

}
