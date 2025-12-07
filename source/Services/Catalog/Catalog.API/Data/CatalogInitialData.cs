using Marten.Schema;

namespace Catalog.API.Data
{
    public class CatalogInitialData : IInitialData
    {
        public async Task Populate(IDocumentStore store, CancellationToken cancellation)
        {
            using var session = store.LightweightSession();
            if(await session.Query<Product>().AnyAsync())
            {
                return;
            }
            session.Store(GetSeedingProductData());
            await session.SaveChangesAsync();
        }

        public static IEnumerable<Product> GetSeedingProductData()
        {
            return new List<Product>() {
                new Product()
                    {
                        Id = new Guid(),
                        Name = "Iphone 15 Promax",
                        Description = "New golden Iphone 15 Promax from Apple",
                        Categories = new List<string>{"Apple", "Electronic"},
                        ImageFile = "default",
                        Price = 31000000,
                    },
                new Product()
                    {
                        Id = new Guid(),
                        Name = "MSI Thin GF63-SRC",
                        Description = "Latop MSI with the most suitable price for student",
                        Categories = new List<string>{"Laptop", "Electronic"},
                        ImageFile = "default",
                        Price = 25000000,
                    }
            };
        }
    }
}
