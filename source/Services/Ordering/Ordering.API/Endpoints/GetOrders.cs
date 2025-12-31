using BuildingBlocks.Pagination;
using Ordering.Application.Orders.Queries.GetOrders;

namespace Ordering.API.Endpoints
{
    //public record GetOrdersRequest(PaginationRequest PaginationRequest);
    public record GetOrdersResponse(PaginatedResult<OrderDto> Orders);
    public class GetOrders : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/orders", async ([AsParameters] PaginationRequest request, ISender sender) =>
            {
                var query = new GetOrdersQuery(request);

                var result = await sender.Send(query);

                var response = result.Adapt<GetOrdersResponse>();
            })
            .WithName("Get Orders")
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status200OK)
            .WithSummary("Get orders")
            .WithDescription("Get list orders with pagination");
        }
    }
}
