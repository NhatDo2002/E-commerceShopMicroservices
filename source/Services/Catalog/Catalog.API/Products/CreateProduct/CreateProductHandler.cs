namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand(string Name, List<string> Categories, string Description, string ImageFile, decimal Price) 
        : ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Product name is required!");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Product price must be greater than 0!");
        }
    }

    internal class CreateProductCommandHandler(
            IDocumentSession session,
            ILogger<CreateProductCommandHandler> logger
        )
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("CreateProductCommandHandler.Handle is called with {@Command}", command);

            //Create Product entity from command object
            var product = new Product
            {
                Name = command.Name,
                Categories = command.Categories,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price
            };

            // Save to the database
            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);
            // Return CreateProductResult result
            return new CreateProductResult(product.Id);
        }
    }
}
