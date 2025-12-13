namespace Discount.Grpc.Services
{
    public class DiscountService(
            DiscountContext dbContext,
            ILogger<DiscountService> logger
        ) 
        : DiscountProtoService.DiscountProtoServiceBase
    {
        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbContext.Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName);
            if(coupon is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Entity was not found with provided name"));
            }
            var couponModel = coupon.Adapt<CouponModel>();
            logger.LogInformation("Get discount successfully with Product name: {productName}", couponModel.ProductName);
            return couponModel;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();
            if(coupon is null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request parameter"));
            }
            dbContext.Coupons.Add(coupon);
            await dbContext.SaveChangesAsync();
            var couponModel = coupon.Adapt<CouponModel>();
            logger.LogInformation("Create discount successfully with Product name: {productName} and Amount: {amount}", couponModel.ProductName, couponModel.Amount);
            return couponModel;
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var updateCoupon = request.Coupon.Adapt<Coupon>();
            if (updateCoupon is null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request parameter"));
            }
            var coupon = await dbContext.Coupons.FirstOrDefaultAsync(c => c.Id == updateCoupon.Id);
            if(coupon is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Entity was not found"));
            }
            coupon.ProductName = updateCoupon.ProductName;
            coupon.Description = updateCoupon.Description;
            coupon.Amount = updateCoupon.Amount;
            await dbContext.SaveChangesAsync();
            var couponModel = updateCoupon.Adapt<CouponModel>();
            logger.LogInformation("Update discount successfully with Id: {id}", couponModel.Id);
            return couponModel;
        }

        public override async Task<DeleteDiscountRepsonse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbContext.Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName);
            if (coupon is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Entity was not found"));
            }
            dbContext.Coupons.Remove(coupon);
            await dbContext.SaveChangesAsync();
            logger.LogInformation("Delete discount successfully with Product name: {productName}", request.ProductName);
            return new DeleteDiscountRepsonse { IsSuccess = true };
        }
    }
}
