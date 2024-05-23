using JobFinder.Api.Models.DTOs;
using JobFinder.Api.Models.ValueObjects;
using JobFinder.Api.Utilities;

namespace JobFinder.Api.Services;

public interface IJobService
{
    Task<Result<SearchJobsResponse>> SearchJobsAsync(JobSearchParameters parameters);
    Task<Result<JobDescription>> GetJobDescriptionAsync(string jobDescriptionId);
    Task<Result<CredentialsResponse>> SelectJobAsync(string challengeType, string challengeKey, string challengeAnswer, string correlationId);
    Task<Result> SubmitApplicantAsync(string applicationId, Applicant applicant, string authCode, string correlationId);
    Task<Result<DocumentResponse>> UploadDocumentAsync(string applicationId, Document document, string authCode, string correlationId);
    Task<Result<ApplicationResponse>> SubmitApplicationAsync(string applicationId, JobApplication jobApplication, string authCode, string correlationId);
}
