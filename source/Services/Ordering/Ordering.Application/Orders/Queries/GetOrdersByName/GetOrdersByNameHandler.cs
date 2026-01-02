namespace Ordering.Application.Orders.Queries.GetOrderByName
{
    public class GetOrdersByNameHandler(IApplicationDbContext dbContext)
        : IQueryHandler<GetOrdersByNameQuery, GetOrdersByNameResult>
    {
        public async Task<GetOrdersByNameResult> Handle(GetOrdersByNameQuery request, CancellationToken cancellationToken)
        {
            var orderName = OrderName.Of(request.Name);
            var orders = await dbContext.Orders
                                   .Include(o => o.OrderItems)
                                   .Where(o => o.OrderName == (orderName))
                                   .AsNoTracking()
                                   .ToListAsync(cancellationToken);

            return new GetOrdersByNameResult(orders.ConvertToListOrderDtos());
        }
    }
}
