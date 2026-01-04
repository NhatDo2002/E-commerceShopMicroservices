using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Ordering.Domain.Abstractions;


namespace Ordering.Infrastructure.Data.Interceptors
{
    class DispatchDomainEventInterceptor(IMediator mediator) : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            DomainEventPublish(eventData.Context).GetAwaiter().GetResult();
            return base.SavingChanges(eventData, result);
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            await DomainEventPublish(eventData.Context);
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private async Task DomainEventPublish(DbContext? context)
        {
            if (context == null) return;
            var aggregates = context.ChangeTracker
                                    .Entries<IAggregate>()
                                    .Where(a => a.Entity.DomainEvents.Any())
                                    .Select(a => a.Entity);

            //Get all domain events of all aggregates
            var domainEvents = aggregates
                                .SelectMany(a => a.DomainEvents)
                                .ToList();

            //Clear all domain events before dispatch events to prevent duplicate publish event
            aggregates.ToList().ForEach(a => a.ClearDomainEvents());

            foreach(var domainEvent in domainEvents)
            {
                await mediator.Publish(domainEvent);
            }
        }
    }
}
