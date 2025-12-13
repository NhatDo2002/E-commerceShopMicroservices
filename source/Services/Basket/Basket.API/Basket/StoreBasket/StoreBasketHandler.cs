using Discount.Grpc;

namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
    public record StoreBasketResult(string UserName);

    public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidator()
        {
            RuleFor(x => x.Cart).NotNull().WithMessage("Shopping cart is required!");
            RuleFor(x => x.Cart).NotEmpty().WithMessage("Shopping cart information is required!");
            RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("User name is required!");
            RuleFor(x => x.Cart.Items).NotNull().WithMessage("Shopping cart items are required!");
            RuleFor(x => x.Cart.Items).Must(items => items.Count > 0).WithMessage("Shopping cart must have at least one item!");
        }
    }
    
    internal class StoreBasketCommandHandler(
            IBasketRepository repository,
            DiscountProtoService.DiscountProtoServiceClient discountClient

        )
        : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            await DeductDiscount(command.Cart, cancellationToken);
            var basket = await repository.StoreBasket(command.Cart, cancellationToken);
            return new StoreBasketResult(basket.UserName);
        }

        private async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellationToken)
        {
            foreach (var item in cart.Items)
            {
                var getDiscount = await discountClient.GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName }, cancellationToken: cancellationToken);
                if (getDiscount != null)
                {
                    item.Price -= getDiscount.Amount;
                }
            }
        }
    }
}
