using FluentValidation;

namespace Demo.Application.Patients.Command
{

    public class DeletePatientCommandValidator : AbstractValidator<DeletePatientCommand>
    {
        public DeletePatientCommandValidator()
        {
            RuleFor(v => v.Id).GreaterThan(0);
        }
    }
}