namespace Ordering.Infrastructure.Data.Configurations
{
    class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(oi => oi.Id);
            builder.Property(oi => oi.Id).HasConversion(
                    orderItemId => orderItemId.Value,
                    dbid => OrderItemId.Of(dbid)
                );
            builder.Property(oi => oi.Price).IsRequired();
            builder.Property(oi => oi.Quantity).IsRequired();

            builder.HasOne<Product>()
                   .WithMany()
                   .HasForeignKey(oi => oi.ProductId);
        }
    }
}
