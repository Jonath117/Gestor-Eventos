using FluentValidation;

namespace Events.Application.Features.Events.RegisterParticipant;

public class RegisterParticipantValidator : AbstractValidator<RegisterParticipantCommand>
{
    public RegisterParticipantValidator()
    {
        RuleFor(x => x.EventId)
            .NotEmpty().WithMessage("Event ID is required.");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full Name is required.")
            .MaximumLength(100).WithMessage("Full Name cannot exceed 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email is required.")
            .MaximumLength(150).WithMessage("Email cannot exceed 150 characters.");
    }
}
