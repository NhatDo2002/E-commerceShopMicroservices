namespace Ordering.Application.Orders.Commands.DeleteOrder
{
    public class DeleteOrderHandler(IApplicationDbContext dbContext)
        : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
    {
        public async Task<DeleteOrderResult> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
        {
            var orderId = OrderId.Of(command.OrderId);

            var order = await dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
            if(order == null)
            {
                throw new OrderNotFoundException("Order", orderId);
            }
            
            dbContext.Orders.Remove(order);
            await dbContext.SaveChangesAsync(cancellationToken);
            return new DeleteOrderResult(true);
        }
    }
}
