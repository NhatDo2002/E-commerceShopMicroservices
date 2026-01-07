using BuildingBlocks.Messaging.Events;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Ordering.Application.Orders.Commands.CreateOrder;
using Ordering.Domain.Enums;

namespace Ordering.Application.Orders.EventHandlers.Integration
{
    public class BasketCheckoutEventHandler
        (ISender sender, ILogger<BasketCheckoutEventHandler> logger)
        : IConsumer<BasketCheckoutEvent>
    {
        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            logger.LogInformation("Integration event handle: {IntegrationEvent}", context.Message.GetType().Name);
            var command = MakeAdaptToCreateOrderCommand(context.Message);
            
            await sender.Send(command, context.CancellationToken);
        }

        private CreateOrderCommand MakeAdaptToCreateOrderCommand(BasketCheckoutEvent basketCheckoutEvent)
        {
            var addressDto = new AddressDto(
                    FirstName: basketCheckoutEvent.FirstName,
                    LastName: basketCheckoutEvent.LastName,
                    EmailAddress: basketCheckoutEvent.EmailAddress,
                    AddressLine: basketCheckoutEvent.AddressLine,
                    Country: basketCheckoutEvent.Country,
                    State: basketCheckoutEvent.State,
                    ZipCode: basketCheckoutEvent.ZipCode
                );
            var paymentDto = new PaymentDto(
                    CardName: basketCheckoutEvent.CardName,
                    CardNumber: basketCheckoutEvent.CardNumber,
                    Expiration: basketCheckoutEvent.Expiration,
                    Cvv: basketCheckoutEvent.CVV,
                    PaymentMethod: basketCheckoutEvent.PaymentMethod
                );
            var orderId = Guid.NewGuid();

            var orderDto = new OrderDto(
                    Id: orderId,
                    CustomerId: basketCheckoutEvent.CustomId,
                    OrderName: basketCheckoutEvent.UserName,
                    ShippingAddress: addressDto,
                    BillingAddress: addressDto,
                    Payment: paymentDto,
                    Status: OrderStatus.Pending,
                    TotalAmount: basketCheckoutEvent.TotalPrice,
                    OrderItems: new List<OrderItemDto>()
                    {
                        new OrderItemDto
                        (
                            OrderId: orderId,
                            ProductId: new Guid("22b38c70-af56-4783-8ccc-4c1327aac82b"),
                            Quantity: 2,
                            Price: 1500
                        ),
                        new OrderItemDto
                        (
                            OrderId: orderId,
                            ProductId: new Guid("2d8b65ef-b11f-40be-a626-578106a3ef4d"),
                            Quantity: 1,
                            Price: 600
                        ),

                    }
                );
            return new CreateOrderCommand(orderDto);
        }
    }
}
