using FluentValidation;
using RestaurantReservation.API.DTO_s.OrderDto;

namespace RestaurantReservation.API.Validators.OrderValidator
{
    public class OrderCreateValidator : AbstractValidator<OrderCreateDto>
    {
        public OrderCreateValidator()
        {
            RuleFor(x => x.ReservationId)
                       .GreaterThan(0).WithMessage("Reservation ID must be a positive integer");

            RuleFor(x => x.EmployeeId)
                .GreaterThan(0).WithMessage("Employee ID must be a positive integer");

            RuleFor(x => x.OrderDate)
                .NotEmpty().WithMessage("Order date is required")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Order date cannot be in the future");

            RuleFor(x => x.TotalAmount)
                .GreaterThanOrEqualTo(0).WithMessage("Total amount must be zero or greater")
                .ScalePrecision(2, 18).WithMessage("Total amount can have a maximum of 2 decimal places and up to 18 digits in total.");
        }
    }
}
