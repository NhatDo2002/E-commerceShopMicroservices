
namespace Catalog.API.Products.UpdateProductById
{
    public record UpdateProductByIdCommand(Guid Id, string Name, List<string> Categories, string Description, string ImageFile, decimal Price) : ICommand<UpdateProductByIdResult>;
    public record UpdateProductByIdResult(Product Product);

    public class UpdateProductByIdCommandValidator : AbstractValidator<UpdateProductByIdCommand>
    {
        public UpdateProductByIdCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Product id is required!");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Product name is required!");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Product price must be greater than 0!");
        }
    }

    internal class UpdateProductByIdCommandHandler(
            IDocumentSession session,
            ILogger<UpdateProductByIdCommandHandler> logger
        )
        : ICommandHandler<UpdateProductByIdCommand, UpdateProductByIdResult>
    {
        public async Task<UpdateProductByIdResult> Handle(UpdateProductByIdCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            logger.LogInformation("UpdateProductByIdCommandHandler.Handle is called with {@Request}", request);
            var getProduct = await session.LoadAsync<Product>(request.Id, cancellationToken);
            if(getProduct is null)
            {
                throw new ProductNotFoundException(request.Id);
            }
            getProduct.Name = request.Name;
            getProduct.Categories = request.Categories;
            getProduct.Description = request.Description;
            getProduct.ImageFile = request.ImageFile;
            getProduct.Price = request.Price;
            session.Update(getProduct);
            await session.SaveChangesAsync(cancellationToken);
            return new UpdateProductByIdResult(getProduct);
        }
    }
}
