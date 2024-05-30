using FluentValidation;

namespace Demo.Application.Orders.Command
{

    public class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
    {
        public DeleteOrderCommandValidator()
        {
            RuleFor(v => v.Id).GreaterThan(0);
        }
    }
}