
using Microsoft.Extensions.Logging;

namespace Ordering.Application.Orders.EventHandlers
{
    public class OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger)
        : INotificationHandler<OrderCreatedEvent>
    {
        public Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation("Domain event handle: {DomainEvent}", notification.GetType().Name);
            return Task.CompletedTask;
        }
    }
}
