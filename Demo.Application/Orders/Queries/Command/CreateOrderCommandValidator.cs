using Demo.Application.Orders.Command;
using FluentValidation;

namespace Demo.Application.Patients.Command
{

    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(v => v.Order.OrderItems)
                .NotEmpty();

            RuleFor(v => v.Order.Customer)
               .NotEmpty();
        }
    }
}