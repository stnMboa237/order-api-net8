using FluentValidation;
using OrderSystem.Application;

namespace OrderSystem.Application.Features.Order.Commands.Update
{
    public class UpdateOrderCommandValidator : ValidatorBase<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty().WithMessage("OrderId is required.");

            RuleFor(x => x.ClientName)
                .NotEmpty().WithMessage("Customer name is required.")
                .Length(3, 100).WithMessage("Customer name must be between 3 and 100 characters.");

            RuleFor(x => x.OrderDate)
                .NotEmpty().WithMessage("Order date is required.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Order date cannot be in the future.");

            RuleForEach(x => x.Items)
                .ChildRules(items =>
                {
                    items.RuleFor(i => i.ItemName).NotEmpty().WithMessage("Item name is required.");
                    items.RuleFor(i => i.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0.");
                    items.RuleFor(i => i.UnitPrice).GreaterThanOrEqualTo(0).WithMessage("Unit price cannot be negative.");
                });

            RuleFor(x => x.Items)
                .Must(items => items != null && items.Count >= 1).WithMessage("At least one item is required.");
        }
    }
}

