using Ordering.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data.Configurations
{
    class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).HasConversion(
                    orderId => orderId.Value,
                    dbid => OrderId.Of(dbid)
                );

            builder.Property(o => o.OrderName)
                   .HasMaxLength(200)
                   .HasConversion(
                        orderName => orderName.Value,
                        dbname => OrderName.Of(dbname)
                   );

            builder.HasOne<Customer>()
                   .WithMany()
                   .HasForeignKey(o => o.CustomerId);
            builder.HasMany<OrderItem>()
                   .WithOne()
                   .HasForeignKey(o => o.OrderId);

            builder.ComplexProperty(
                    o => o.BillingAddress,
                    billingBuilder =>
                    {
                        billingBuilder.Property(bb => bb.EmailAddress)
                                      .HasMaxLength(100)
                                      .IsRequired();
                        billingBuilder.Property(bb => bb.AddressLine)
                                      .HasMaxLength(150)
                                      .IsRequired();
                        billingBuilder.Property(bb => bb.State)
                                      .HasMaxLength(100);
                        billingBuilder.Property(bb => bb.Country)
                                      .HasMaxLength(100);
                        billingBuilder.Property(bb => bb.ZipCode)
                                      .HasMaxLength(100);
                        billingBuilder.Property(bb => bb.FirstName)
                                      .HasMaxLength(100)
                                      .IsRequired();
                        billingBuilder.Property(bb => bb.LastName)
                                      .HasMaxLength(100)
                                      .IsRequired();
                    }
                );

            builder.ComplexProperty(
                    o => o.ShippingAddress,
                    shippingBuilder =>
                    {
                        shippingBuilder.Property(bb => bb.EmailAddress)
                                      .HasMaxLength(100)
                                      .IsRequired();
                        shippingBuilder.Property(bb => bb.AddressLine)
                                      .HasMaxLength(150)
                                      .IsRequired();
                        shippingBuilder.Property(bb => bb.State)
                                      .HasMaxLength(100);
                        shippingBuilder.Property(bb => bb.Country)
                                      .HasMaxLength(100);
                        shippingBuilder.Property(bb => bb.ZipCode)
                                      .HasMaxLength(100);
                        shippingBuilder.Property(bb => bb.FirstName)
                                      .HasMaxLength(100)
                                      .IsRequired();
                        shippingBuilder.Property(bb => bb.LastName)
                                      .HasMaxLength(100)
                                      .IsRequired();
                    }
                );

            builder.ComplexProperty(
                    o => o.Payment,
                    paymentBuilder =>
                    {
                        paymentBuilder.Property(pb => pb.CardName)
                                      .HasMaxLength(100)
                                      .IsRequired();
                        paymentBuilder.Property(pb => pb.CardNumber)
                                      .HasMaxLength(50)
                                      .IsRequired();
                        paymentBuilder.Property(pb => pb.Expiration)
                                      .IsRequired();
                        paymentBuilder.Property(pb => pb.CVV )
                                      .HasMaxLength(3)
                                      .IsRequired();
                        paymentBuilder.Property(pb => pb.PaymentMethod);
                    }
                );

            builder.Property(o => o.Status)
                   .HasDefaultValue(OrderStatus.Draft)
                   .HasConversion(
                        status => status.ToString(),
                        dbStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), dbStatus)
                   );

            builder.Property(o => o.TotalAmount);
        }
    }
}
