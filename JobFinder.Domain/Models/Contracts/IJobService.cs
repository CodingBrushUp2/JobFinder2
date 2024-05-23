using JobFinder.Domain.Models.DTOs;
using JobFinder.Domain.Models.ValueObjects;
using JobFinder.Domain.Utilities;

namespace JobFinder.Domain.Services;

public interface IJobService
{
    Task<Result<SearchJobsResponse>> SearchJobsAsync(JobSearchParameters parameters);
    Task<Result<JobDescription>> GetJobDescriptionAsync(string jobDescriptionId);
    Task<Result<CredentialsResponse>> SelectJobAsync(string challengeType, string challengeKey, string challengeAnswer, string correlationId);
    Task<Result> SubmitApplicantAsync(string applicationId, Applicant applicant, string authCode, string correlationId);
    Task<Result<DocumentResponse>> UploadDocumentAsync(string applicationId, Document document, string authCode, string correlationId);
    Task<Result<ApplicationResponse>> SubmitApplicationAsync(string applicationId, JobApplication jobApplication, string authCode, string correlationId);
    Task<Result<ApplicationResponse>> GetApplicationAsync(string applicationId, string authCode, string correlationId);
}
