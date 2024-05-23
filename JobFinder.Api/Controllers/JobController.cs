using JobFinder.Api.Services;
using JobFinder.Domain.Models.DTOs;
using JobFinder.Domain.Models.ValueObjects;
using JobFinder.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobFinder.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;
        private readonly ILogger<JobController> _logger;

        public JobController(IJobService jobService, ILogger<JobController> logger)
        {
            _jobService = jobService;
            _logger = logger;
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchJobs([FromForm] JobSearchParameters parameters)
        {
            var result = await _jobService.SearchJobsAsync(parameters);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            _logger.LogError("Error occurred while searching for jobs: {Error}", result.Error);
            return StatusCode(result.StatusCode, result.Error);
        }

        [HttpGet("{jobDescriptionId}")]
        public async Task<IActionResult> GetJobDescription(string jobDescriptionId)
        {
            var result = await _jobService.GetJobDescriptionAsync(jobDescriptionId);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            _logger.LogError("Error occurred while getting job description: {Error}", result.Error);
            return StatusCode(result.StatusCode, result.Error);
        }

        [HttpPost("credentials")]
        public async Task<IActionResult> SelectJob(
                    [FromHeader(Name = "ChallengeType")] string challengeType,
                    [FromHeader(Name = "ChallengeKey")] string challengeKey,
                    [FromHeader(Name = "ChallengeAnswer")] string? challengeAnswer,
                    [FromHeader(Name = "correlation-id")] string? correlationId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _jobService.SelectJobAsync(challengeType, challengeKey, challengeAnswer, correlationId);

            if (result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Value);
            }

            _logger.LogError("Error occurred while selecting job: {Error}", result.Error);
            return StatusCode(result.StatusCode, result.Error);
        }

        [HttpPut("{applicationId}/applicant")]
        public async Task<IActionResult> SubmitApplicant(
                    string applicationId,
                    [FromForm] Applicant applicant,
                    [FromHeader(Name = "application-auth-code")] string authCode,
                    [FromHeader(Name = "correlation-id")] string correlationId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _jobService.SubmitApplicantAsync(applicationId, applicant, authCode, correlationId);

            if (result.IsSuccess)
            {
                return StatusCode(result.StatusCode);
            }

            _logger.LogError("Error occurred while submitting applicant: {Error}", result.Error);
            return StatusCode(result.StatusCode, result.Error);
        }

        [HttpPost("upload-document/{applicationId}")]
        public async Task<IActionResult> UploadDocument(
                    string applicationId,
                    [FromBody] Document document,
                    [FromHeader(Name = "application-auth-code")] string authCode,
                    [FromHeader(Name = "correlation-id")] string correlationId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _jobService.UploadDocumentAsync(applicationId, document, authCode, correlationId);

            if (result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Value);
            }

            _logger.LogError("Error occurred while uploading document: {Error}", result.Error);
            return StatusCode(result.StatusCode, result.Error);
        }

        [HttpPost("submit/{applicationId}")]
        public async Task<IActionResult> SubmitApplication(
                    string applicationId,
                    [FromForm] JobApplication jobApplication,
                    [FromHeader(Name = "application-auth-code")] string authCode,
                    [FromHeader(Name = "correlation-id")] string correlationId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _jobService.SubmitApplicationAsync(applicationId, jobApplication, authCode, correlationId);

            if (result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Value);
            }

            _logger.LogError("Error occurred while submitting application: {Error}", result.Error);
            return StatusCode(result.StatusCode, result.Error);
        }

        [HttpGet("application/{applicationId}")]
        public async Task<IActionResult> GetApplication(
                string applicationId,
                [FromHeader(Name = "application-auth-code")] string authCode,
                [FromHeader(Name = "correlation-id")] string correlationId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _jobService.GetApplicationAsync(applicationId, authCode, correlationId);

            if (result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Value);
            }

            _logger.LogError("Error occurred while submitting application: {Error}", result.Error);
            return StatusCode(result.StatusCode, result.Error);
        }
    } 
}
