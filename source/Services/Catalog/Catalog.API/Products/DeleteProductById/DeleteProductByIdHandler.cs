
namespace Catalog.API.Products.DeleteProductById
{
    public record DeleteProductByIdCommand(Guid Id) : ICommand<DeleteProductByIdResult>;
    public record DeleteProductByIdResult(bool IsSuccess);

    public class DeleteProductByIdValidator : AbstractValidator<DeleteProductByIdCommand>
    {
        public DeleteProductByIdValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Product id is required!");
        }
    }
    internal class DeleteProductByIdCommandHandler(
            IDocumentSession session,
            ILogger<DeleteProductByIdCommandHandler> logger
        )
        : ICommandHandler<DeleteProductByIdCommand, DeleteProductByIdResult>
    {
        public async Task<DeleteProductByIdResult> Handle(DeleteProductByIdCommand request, CancellationToken cancellationToken)
        {
            if(request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            logger.LogInformation("DeleteProductByIdCommandHandler.Handle is called with {@Request}", request);
            var getProduct = await session.LoadAsync<Product>(request.Id, cancellationToken);
            if (getProduct is null)
            {
                throw new ProductNotFoundException(request.Id);
            }
            session.Delete(getProduct);
            await session.SaveChangesAsync(cancellationToken);
            return new DeleteProductByIdResult(true);
        }
    }
}
