using FluentValidation;

namespace Demo.Application.Patients.Command
{

    public class CreatePatientCommandValidator : AbstractValidator<CreatePatientCommand>
    {
        public CreatePatientCommandValidator()
        {
            RuleFor(v => v.Patient.FirstName)
                .MaximumLength(200)
                .MinimumLength(6)
                .NotEmpty();

            RuleFor(v => v.Patient.LastName)
               .MaximumLength(200)
               .MinimumLength(6)
               .NotEmpty();
        }
    }
}