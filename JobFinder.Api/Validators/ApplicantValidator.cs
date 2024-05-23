using FluentValidation;
using JobFinder.Domain.Models.ValueObjects;

namespace JobFinder.Api.Validators;

public class ApplicantValidator : AbstractValidator<Applicant>
{
    public ApplicantValidator()
    {
        RuleFor(a => a.FirstName).NotEmpty();
        RuleFor(a => a.LastName).NotEmpty();
        RuleFor(a => a.Email).NotEmpty().EmailAddress();
        RuleFor(a => a.TelephoneNumber).NotEmpty();
    }
}
