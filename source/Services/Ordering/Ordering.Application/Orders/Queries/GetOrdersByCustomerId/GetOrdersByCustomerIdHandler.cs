using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.Queries.GetOrdersByCustomerId
{
    public class GetOrdersByCustomerIdHandler(IApplicationDbContext dbContext)
        : IQueryHandler<GetOrdersByCustomerIdQuery, GetOrdersByCustomerIdResult>
    {
        public async Task<GetOrdersByCustomerIdResult> Handle(GetOrdersByCustomerIdQuery query, CancellationToken cancellationToken)
        {
            var orders = await dbContext.Orders
                                  .Include(o => o.OrderItems)
                                  .AsNoTracking()
                                  .Where(o => o.CustomerId.Value == query.CustomerId)
                                  .OrderBy(o => o.OrderName.Value)
                                  .ToListAsync(cancellationToken);
            return new GetOrdersByCustomerIdResult(orders.ConvertToListOrderDtos());
        }
    }
}
