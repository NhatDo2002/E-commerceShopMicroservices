namespace Ordering.Infrastructure.Data.Configurations
{
    class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasConversion(
                    customerId => customerId.Value,
                    dbid => CustomerId.Of(dbid)
                );
            builder.Property(c => c.Name).IsRequired().HasMaxLength(250);
            builder.Property(c => c.Email).IsRequired().HasMaxLength(150);
            builder.HasIndex(c => c.Email).IsUnique();
        }
    }
}
