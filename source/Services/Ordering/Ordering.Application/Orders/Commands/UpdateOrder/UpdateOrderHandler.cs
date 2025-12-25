namespace Ordering.Application.Orders.Commands.UpdateOrder
{
    public class UpdateOrderHandler(IApplicationDbContext dbContext)
        : ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
    {
        public async Task<UpdateOrderResult> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
        {
            var orderId = OrderId.Of(command.OrderRequest.Id);

            var order = await dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
            if(order == null)
            {
                throw new OrderNotFoundException("Order", orderId);
            }

            UpdateOrderInformation(order, command.OrderRequest);
            dbContext.Orders.Update(order);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new UpdateOrderResult(true);
        }

        private void UpdateOrderInformation(Order order, OrderDto orderRequest)
        {

            var updateBillingAddress = Address.Of(
                    orderRequest.BillingAddress.FirstName,
                    orderRequest.BillingAddress.LastName,
                    orderRequest.BillingAddress.EmailAddress,
                    orderRequest.BillingAddress.AddressLine,
                    orderRequest.BillingAddress.Country,
                    orderRequest.BillingAddress.State,
                    orderRequest.BillingAddress.ZipCode
                );
            var updateShippingAddress = Address.Of(
                    orderRequest.ShippingAddress.FirstName,
                    orderRequest.ShippingAddress.LastName,
                    orderRequest.ShippingAddress.EmailAddress,
                    orderRequest.ShippingAddress.AddressLine,
                    orderRequest.ShippingAddress.Country,
                    orderRequest.ShippingAddress.State,
                    orderRequest.ShippingAddress.ZipCode
                );

            var updatePayment = Payment.Of(
                    orderRequest.Payment.CardName,
                    orderRequest.Payment.CardNumber,
                    orderRequest.Payment.Expiration,
                    orderRequest.Payment.Cvv,
                    orderRequest.Payment.PaymentMethod
                );

            order.Update(
                    OrderName.Of(orderRequest.OrderName),
                    CustomerId.Of(orderRequest.CustomerId),
                    updateBillingAddress,
                    updateShippingAddress,
                    updatePayment,
                    orderRequest.Status
                );
        }
    }
}
