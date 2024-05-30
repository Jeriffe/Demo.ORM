using FluentValidation;

namespace Demo.Application.Orders.Command
{

    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(v => v.Order.Id).GreaterThan(0);

            RuleFor(v => v.Order.OrderItems)
                .NotEmpty();

            RuleFor(v => v.Order.Customer)
               .NotEmpty();
        }
    }
}