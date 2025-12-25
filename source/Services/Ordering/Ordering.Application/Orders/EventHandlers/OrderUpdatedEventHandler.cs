
namespace Ordering.Application.Orders.EventHandlers
{
    public class OrderUpdatedEventHandler(ILogger<OrderUpdatedEventHandler> logger)
        : INotificationHandler<OrderUpdatedEvent>
    {
        public Task Handle(OrderUpdatedEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation("Domain event handle: {DomainEvent}", notification.GetType().Name);
            return Task.CompletedTask;
        }
    }
}
