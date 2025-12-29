using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.Queries.GetOrdersByCustomer
{
    public record GetOrdersByCustomerNameQuery(string CustomerName) : IQuery<GetOrdersByCustomerNameResult>;
    public record GetOrdersByCustomerNameResult(IEnumerable<OrderDto> Orders);
}
