namespace Ordering.Application.Orders.Commands.CreateOrder
{
    public record CreateOrderCommandResult (Guid OrderId);

    public record CreateOrderCommand(OrderDto OrderRequest) : ICommand<CreateOrderCommandResult>;

    public class  CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.OrderRequest.OrderName).NotEmpty().WithMessage("Order name must not be empty.");
            RuleFor(x => x.OrderRequest.CustomerId).NotEmpty().WithMessage("Customer must not be empty.");
            RuleFor(x => x.OrderRequest.TotalAmount).GreaterThan(0).WithMessage("Total amount of order must be greater than 0.");
            RuleFor(x => x.OrderRequest.OrderItems).NotEmpty().WithMessage("Order must contain at least one item.");
        }
    }
}
