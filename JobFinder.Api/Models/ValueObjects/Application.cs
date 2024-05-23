using JobFinder.Api.Models.DTOs;

namespace JobFinder.Api.Models.ValueObjects;

public record Application(string JobId, Applicant Applicant, List<Document> Documents);
