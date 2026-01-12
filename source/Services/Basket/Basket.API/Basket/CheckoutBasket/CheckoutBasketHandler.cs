using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace Basket.API.Basket.CheckoutBasket
{
    public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckoutDto) : ICommand<CheckoutBasketResult>;
    public record CheckoutBasketResult(bool IsSuccess);
    
    public class CheckoutBasketValidator : AbstractValidator<CheckoutBasketCommand>
    {
        public CheckoutBasketValidator() 
        { 
            RuleFor(x => x.BasketCheckoutDto).NotNull().WithMessage("Invalid request");
            RuleFor(x => x.BasketCheckoutDto.UserName).NotEmpty().WithMessage("UserName is required");
        }
    }

    internal class CheckoutBasketCommandHandler
        (IBasketRepository basketRepository, IPublishEndpoint publishEndpoint)
        : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
    {
        public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
        {
            var getBasket = await basketRepository.GetBasket(command.BasketCheckoutDto.UserName, cancellationToken);
            if(getBasket is null)
            {
                return new CheckoutBasketResult(false);
            }

            var message = command.Adapt<CheckoutBasketEvent>();
            message.TotalPrice = getBasket.ToTalPrice;

            await publishEndpoint.Publish(message, cancellationToken);

            await basketRepository.DeleteBasket(command.BasketCheckoutDto.UserName);

            return new CheckoutBasketResult(true);
        }
    }
}
