using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Ordering.Domain.Abstractions;

namespace Ordering.Infrastructure.Data.Interceptors
{
    public class AuditableDatabaseInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntity(eventData);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateEntity(eventData);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void UpdateEntity(DbContextEventData eventData) { 
            if(eventData.Context == null)
            {
                return;
            }
            var context = eventData.Context;

            foreach(var entry in context.ChangeTracker.Entries<IEntity>())
            {
                if(entry.State == EntityState.Added)
                {
                    entry.Entity.CreateBy = "nhat.dm";
                    entry.Entity.CreateDate = DateTime.UtcNow;
                }

                if(entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
                {
                    entry.Entity.ModifyBy = "nhat.dm";
                    entry.Entity.ModifyDate = DateTime.UtcNow;
                }
            }
        }
    }
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry)
    {
        return entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified)
        );
    }
}
