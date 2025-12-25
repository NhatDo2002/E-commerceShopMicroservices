using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Models;

namespace Ordering.Application.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Product> Products { get; }
        DbSet<Order> Orders { get; }
        DbSet<OrderItem> OrderItems { get; }
        DbSet<Customer> Customers { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
