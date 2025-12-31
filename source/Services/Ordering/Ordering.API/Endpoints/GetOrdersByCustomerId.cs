
using Ordering.Application.Orders.Queries.GetOrdersByCustomerId;

namespace Ordering.API.Endpoints
{
    //public record GetOrdersByCustomerIdRequest(Guid CustomerId);
    public record GetOrdersByCustomerIdResponse(IEnumerable<OrderDto> Orders);

    public class GetOrdersByCustomerId : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/orders/customer/{customerId}", async (Guid customerId, ISender sender) =>
            {
                var query = new GetOrdersByCustomerIdQuery(customerId);

                var result = await sender.Send(query);

                var response = result.Adapt<GetOrdersByCustomerIdResponse>();

                return Results.Ok(response);
            })
            .WithName("Get Orders By Customer")
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status200OK)
            .WithSummary("Get orders by customer")
            .WithDescription("Get list orders by provided customer id");
        }
    }
}
