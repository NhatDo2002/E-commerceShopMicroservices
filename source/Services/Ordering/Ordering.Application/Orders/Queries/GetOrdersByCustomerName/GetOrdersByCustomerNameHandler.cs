using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.Queries.GetOrdersByCustomer
{
    public class GetOrdersByCustomerNameHandler(IApplicationDbContext dbContext)
        : IQueryHandler<GetOrdersByCustomerNameQuery, GetOrdersByCustomerNameResult>
    {
        //public async Task<GetOrdersByCustomerNameResult> Handle(GetOrdersByCustomerNameQuery query, CancellationToken cancellationToken)
        //{
        //    var orders = await dbContext.Orders
        //                          .Include(o => o.OrderItems)
        //                          .AsNoTracking()
        //                          .Select(o => new
        //                          {
        //                              o,
        //                              CustomerName = dbContext.Customers
        //                                                  .Where(c => c.Id == o.CustomerId)
        //                                                  .Select(c => c.Name)
        //                                                  .FirstOrDefault()
        //                          })
        //                          .ToListAsync(cancellationToken);
        //    var filteredOrders = orders.Where(o => o.CustomerName.Contains(query.CustomerName))
        //                               .Select(o => o.o)
        //                               .ToList();

        //    return new GetOrdersByCustomerNameResult(filteredOrders.ConvertToListOrderDtos());
        //}
        public Task<GetOrdersByCustomerNameResult> Handle(GetOrdersByCustomerNameQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
