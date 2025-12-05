namespace Catalog.API.Products.GetProductById
{
    public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;

    public record GetProductByIdResult(Product Product);

    internal class GetProductByIdQueryHandler(
            IDocumentSession session,
            ILogger<GetProductByIdQueryHandler> logger
        ) 
        : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
    {
        public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetProductByIdQueryHandler.Handle is called with {@Query}", query);
            var getProduct = await session.LoadAsync<Product>(query.Id, cancellationToken);
            if (getProduct != null)
            {
                return new GetProductByIdResult(getProduct);
            }
            else
            {
                throw new ProductNotFoundException(query.Id);
            }
        }
    }
}
