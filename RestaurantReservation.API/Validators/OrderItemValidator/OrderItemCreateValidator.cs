using FluentValidation;
using RestaurantReservation.API.DTO_s.OrderDto;

namespace RestaurantReservation.API.Validators.OrderItemValidator
{
    public class OrderItemCreateValidator : AbstractValidator<OrderItemCreateDto>
    {
        public OrderItemCreateValidator()
        {
            RuleFor(x => x.ItemId)
                .GreaterThan(0).WithMessage("Item ID must be a positive integer");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be a positive integer")
                .LessThanOrEqualTo(100).WithMessage("Quantity must be 100 or less");
        }
    }
}
