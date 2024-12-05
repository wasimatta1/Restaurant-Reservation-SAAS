using FluentValidation;
using RestaurantReservation.API.DTO_s.CustomerDto;

namespace RestaurantReservation.API.Validators.CustomerValidator
{
    public class CustomerUpdateValidator : AbstractValidator<CustomerUpdaetDto>
    {
        public CustomerUpdateValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("FirstName is required")
                .Length(3, 15).WithMessage("First Name must be between 3 and 15 characters");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .Length(3, 15).WithMessage("Last Name must be between 3 and 15 characters");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Email is not valid");

            RuleFor(x => x.PhoneNumber)
                .Must(startWith).WithMessage("Phone number must start with 05")
                .Matches("^[0-9]*$").MaximumLength(10).WithMessage("Phone number must be 10 digits");
        }
        private bool startWith(string? phoneNumber)
        {
            return phoneNumber?.StartsWith("05") ?? false;
        }
    }
}
