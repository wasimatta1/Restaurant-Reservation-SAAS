using FluentValidation;
using RestaurantReservation.API.DTO_s.ReservationDto;

namespace RestaurantReservation.API.Validators.ReservationValidator
{
    public class ReservationUpdateValidator : AbstractValidator<ReservationUpdateDto>
    {
        public ReservationUpdateValidator()
        {
            RuleFor(x => x.CustomerId)
                 .GreaterThan(0).WithMessage("Customer ID must be a positive integer");

            RuleFor(x => x.TableId)
                .GreaterThan(0).WithMessage("Table ID must be a positive integer");

            RuleFor(x => x.ReservationDate)
                .GreaterThanOrEqualTo(DateTime.Now).WithMessage("Reservation date must be in the future");

            RuleFor(x => x.PartySize)
                .GreaterThan(0).WithMessage("Party size must be a positive integer")
                .LessThanOrEqualTo(20).WithMessage("Party size cannot exceed 20 guests");
        }
    }
}
