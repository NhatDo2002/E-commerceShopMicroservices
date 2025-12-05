
namespace Catalog.API.Products.DeleteProductById
{
    //public record DeleteProductByIdRequest(Guid Id);
    public record DeleteProductByIdResponse(bool IsSuccess);
    public class DeleteProductByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/products/{id}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new DeleteProductByIdCommand(id));
                var response = result.Adapt<DeleteProductByIdResponse>();
                return Results.Ok(response);
            })
            .WithName("DeleteProductById")
            .Produces<DeleteProductByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Product by id")
            .WithDescription("Delete a product in catalog with provided id");
        }
    }
}
