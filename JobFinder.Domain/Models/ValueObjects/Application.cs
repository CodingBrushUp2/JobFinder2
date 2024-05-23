using JobFinder.Domain.Models.DTOs;

namespace JobFinder.Domain.Models.ValueObjects;

public record Application(string JobId, Applicant Applicant, List<Document> Documents);
