namespace Ordering.Application.Orders.Commands.UpdateOrder
{
    public record UpdateOrderCommand(OrderDto OrderRequest) : ICommand<UpdateOrderResult>;

    public record UpdateOrderResult(bool IsSuccess);

    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(x => x.OrderRequest.OrderName).NotEmpty().WithMessage("Order name cannot be empty.");
            RuleFor(x => x.OrderRequest.CustomerId).NotEmpty().WithMessage("Customer ID cannot be empty.");
            RuleFor(x => x.OrderRequest.TotalAmount).GreaterThan(0).WithMessage("Total amount must be greater than zero.");
            RuleFor(x => x.OrderRequest.OrderItems).NotEmpty().WithMessage("Order must have at least one item.");
        }
    }
}
