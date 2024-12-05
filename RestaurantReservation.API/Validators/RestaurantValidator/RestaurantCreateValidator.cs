using FluentValidation;
using RestaurantReservation.API.DTO_s.RestaurantDto;

namespace RestaurantReservation.API.Validators.RestaurantValidator
{
    public class RestaurantCreateValidator : AbstractValidator<RestaurantInfoDto>
    {
        public RestaurantCreateValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Restaurant name cannot be empty")
            .Length(1, 50).WithMessage("Restaurant name must be between 1 and 50 characters");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address cannot be empty")
                .Length(1, 200).WithMessage("Address must be between 1 and 200 characters");

            RuleFor(x => x.PhoneNumber)
                .Must(startWith).WithMessage("Phone number must start with 05")
                .Matches("^[0-9]*$").MaximumLength(10).WithMessage("Phone number must be 10 digits");

            RuleFor(x => x.OpeningHours)
                .Matches(@"^([01]?[0-9]|2[0-3]):([0-5][0-9])\s*-\s*([01]?[0-9]|2[0-3]):([0-5][0-9])$")
                .WithMessage("Opening hours must be in the format HH:mm - HH:mm (e.g., 09:00 - 22:00)");
        }

        private bool startWith(string? phoneNumber)
        {
            return phoneNumber?.StartsWith("05") ?? false;
        }
    }
}
