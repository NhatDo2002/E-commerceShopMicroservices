using Ordering.Application.Orders.Commands.CreateOrder;

namespace Ordering.API.Endpoints
{
    public record CreateOrderRequest(OrderDto OrderRequest);
    public record CreateOrderResponse(Guid OrderId);

    public class CreateOrder : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/orders", async (CreateOrderRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateOrderCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<CreateOrderResponse>();

                return Results.Created($"/orders/{response.OrderId}", response);
            })
            .WithName("Add Order")
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status200OK)
            .WithSummary("Add customer's order")
            .WithDescription("Add customer's order with provided information");
        }
    }
}
