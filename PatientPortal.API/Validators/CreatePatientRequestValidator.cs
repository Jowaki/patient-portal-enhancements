namespace PatientPortal.API.Validators;

using FluentValidation;
using PatientPortal.Core.DTOs;

public class CreatePatientRequestValidator : AbstractValidator<CreatePatientRequest>
{
    public CreatePatientRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required.")
            .LessThan(DateTime.UtcNow.Date).WithMessage("Date of birth must be in the past.")
            .GreaterThan(DateTime.UtcNow.AddYears(-130)).WithMessage("Date of birth cannot be more than 130 years ago.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\d{3}-\d{4}$|^\d{3}-\d{3}-\d{4}$|^\(\d{3}\)\s\d{3}-\d{4}$")
            .WithMessage("Phone number must be a valid format (e.g. 555-1234 or 555-123-4567).");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.");
    }
}