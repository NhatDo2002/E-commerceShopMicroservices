
namespace Catalog.API.Products.UpdateProductById
{
    public record UpdateProductByIdRequest(Guid Id, string Name, List<string> Categories, string Description, string ImageFile, decimal Price);
    public record UpdateProductByIdResponse(Product Product);
    public class UpdateProductByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/products", async (UpdateProductByIdRequest request, ISender sender) =>
            {
                var command = request.Adapt<UpdateProductByIdCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<UpdateProductByIdResponse>();
                return Results.Ok(response);
            })
            .WithName("UpdateProductById")
            .Produces<UpdateProductByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Upade a product")
            .WithDescription("Update a product in the catalog with the provided details");
        }
    }
}
