using FluentValidation;

namespace Demo.Application.Patients.Command
{

    public class UpdatePatientCommandValidator : AbstractValidator<UpdatePatientCommand>
    {
        public UpdatePatientCommandValidator()
        {
            RuleFor(v => v.Patient.PatientId).GreaterThan(0);

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