namespace Ordering.Application.Orders.Commands.CreateOrder
{
    public class CreateOrderHandler(IApplicationDbContext dbContext)
        : ICommandHandler<CreateOrderCommand, CreateOrderCommandResult>
    {
        public async Task<CreateOrderCommandResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var order = CreateNewOrder(command.OrderRequest);

            dbContext.Orders.Add(order);
            await dbContext.SaveChangesAsync(cancellationToken);
            return new CreateOrderCommandResult(order.Id.Value);
        }

        private Order CreateNewOrder(OrderDto request)
        {
            var billingAddress = Address.Of(
                    request.BillingAddress.FirstName,
                    request.BillingAddress.LastName,
                    request.BillingAddress.EmailAddress,
                    request.BillingAddress.AddressLine,
                    request.BillingAddress.Country,
                    request.BillingAddress.State,
                    request.BillingAddress.ZipCode
                );
            var shippingAddress = Address.Of(
                    request.ShippingAddress.FirstName,
                    request.ShippingAddress.LastName,
                    request.ShippingAddress.EmailAddress,
                    request.ShippingAddress.AddressLine,
                    request.ShippingAddress.Country,
                    request.ShippingAddress.State,
                    request.ShippingAddress.ZipCode
                );

            var payment = Payment.Of(
                    request.Payment.CardName,
                    request.Payment.CardNumber,
                    request.Payment.Expiration,
                    request.Payment.Cvv,
                    request.Payment.PaymentMethod
                );
            var order = Order.Create(
                    orderName: OrderName.Of(request.OrderName),
                    customerId: CustomerId.Of(request.CustomerId),
                    billingAddress: billingAddress,
                    shippingAddress: shippingAddress,
                    payment: payment
                );

            foreach(var item in request.OrderItems)
            {
                order.AddOrderItem(ProductId.Of(item.ProductId), item.Quantity, item.Price);
            }

            return order;
        }
    }
}
