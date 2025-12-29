using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.Queries.GetOrdersByCustomerId
{
    public record GetOrdersByCustomerIdQuery(Guid CustomerId) : IQuery<GetOrdersByCustomerIdResult>;
    public record GetOrdersByCustomerIdResult(IEnumerable<OrderDto> Orders);
}
