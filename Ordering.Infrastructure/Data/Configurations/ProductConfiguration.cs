namespace Ordering.Infrastructure.Data.Configurations
{
    class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasConversion(
                    productId => productId.Value,
                    dbid => ProductId.Of(dbid)
                );
            builder.Property(p => p.Name).IsRequired().HasMaxLength(250);
            builder.Property(p => p.Price).HasDefaultValue(0);
        }
    }
}
