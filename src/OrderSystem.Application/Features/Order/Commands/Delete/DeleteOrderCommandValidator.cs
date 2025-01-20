using FluentValidation;
using OrderSystem.Application;

namespace OrderSystem.Application.Features.Order.Commands.Delete
{
    public class DeleteOrderCommandValidator : ValidatorBase<DeleteOrderCommand>
    {
        public DeleteOrderCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty().WithMessage("OrderId is required.");
        }
    }
}

