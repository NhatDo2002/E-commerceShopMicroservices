namespace Ordering.Domain.Models
{
    public class Order : Aggregate<OrderId>
    {
        private readonly List<OrderItem> _orderItems = new();
        public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();

        public OrderName OrderName { get; private set; } = default!;
        public CustomerId CustomerId { get; private set; } = default!;
        public Address BillingAddress { get; private set; } = default!;
        public Address ShippingAddress { get; private set; } = default!;
        public Payment Payment { get; private set; } = default!;
        public OrderStatus Status { get; private set; } = OrderStatus.Pending;
        public decimal TotalAmount
        {
            get => OrderItems.Sum(item => item.Price * item.Quantity);
            private set { }
        }
        
        public static Order Create(OrderName orderName, CustomerId customerId, Address billingAddress, Address shippingAddress, Payment payment)
        {
            var order = new Order
            {
                Id = OrderId.Of(Guid.NewGuid()),
                OrderName = orderName,
                CustomerId = customerId,
                BillingAddress = billingAddress,
                ShippingAddress = shippingAddress,
                Payment = payment,
                Status = OrderStatus.Pending
            };

            order.AddDomainEvent(new OrderCreatedEvent(order));

            return order;
        }

        public void Update(OrderName orderName, CustomerId customerId, Address billingAddress, Address shippingAddress, Payment payment, OrderStatus status)
        {
            OrderName = orderName;
            CustomerId = customerId;
            BillingAddress = billingAddress;
            ShippingAddress = shippingAddress;
            Payment = payment;
            Status = status;

            AddDomainEvent(new OrderUpdatedEvent(this));
        }

        public void AddOrderItem(ProductId productId, int quantity, decimal price)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);

            var orderItem = new OrderItem(Id, productId, quantity, price);

            _orderItems.Add(orderItem);

        }

        public void RemoveOrderItem(ProductId productId)
        {
            var orderItem = _orderItems.FirstOrDefault(oi => oi.ProductId == productId);
            if(orderItem is not null)
            {
                _orderItems.Remove(orderItem);
            }
        }
    }
}
