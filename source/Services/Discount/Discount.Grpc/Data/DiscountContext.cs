using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data
{
    public class DiscountContext : DbContext
    {
        public DiscountContext(DbContextOptions<DiscountContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Coupon>().HasData(
                new Coupon { Id = 1, ProductName = "IPhone 15 Promax", Description = "IPhone 15 Promax Discount", Amount = 15000 },
                new Coupon { Id = 2, ProductName = "Samsung ZFord S25", Description = "Samsung Christmas Discount", Amount = 20000 }
            );
        }

        public DbSet<Coupon> Coupons { get; set; }
    }
}
