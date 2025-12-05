
namespace Catalog.API.Products.GetProductsByCategory
{
    public record GetProductsByCategoryQuery(string category) : IQuery<GetProductsByCategoryResult>;
    public record GetProductsByCategoryResult(IEnumerable<Product> Products);
    internal class GetProductsByCategoryQueryHandler(
            IDocumentSession session,
            ILogger<GetProductsByCategoryQueryHandler> logger
        )
        : IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
    {
        public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetProductsByCategoryQueryHandler.Handle is called with {@Request}", query);
            var result = await session.Query<Product>().Where(p => p.Categories.Contains(query.category)).ToListAsync();
            return new GetProductsByCategoryResult(result);
        }
    }
}
