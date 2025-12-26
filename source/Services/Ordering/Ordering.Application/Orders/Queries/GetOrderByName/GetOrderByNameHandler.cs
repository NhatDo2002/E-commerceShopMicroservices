namespace Ordering.Application.Orders.Queries.GetOrderByName
{
    public class GetOrderByNameHandler(IApplicationDbContext dbContext)
        : IQueryHandler<GetOrderByNameQuery, GetOrderByNameResult>
    {
        public async Task<GetOrderByNameResult> Handle(GetOrderByNameQuery request, CancellationToken cancellationToken)
        {
            var orders = await dbContext.Orders
                                   .Include(o => o.OrderItems)
                                   .Where(o => o.OrderName.Value.Contains(request.Name))
                                   .AsNoTracking()
                                   .ToListAsync(cancellationToken);

            return new GetOrderByNameResult(orders.ConvertToListOrderDtos());
        }
    }
}
