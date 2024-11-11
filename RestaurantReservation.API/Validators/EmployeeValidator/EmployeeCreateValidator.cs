using FluentValidation;
using RestaurantReservation.API.DTO_s.EmployeeDto;

namespace RestaurantReservation.API.Validators.EmployeeValidator
{
    public class EmployeeCreateValidator : AbstractValidator<EmployeeCreateDto>
    {
        public EmployeeCreateValidator()
        {
            RuleFor(x => x.FirstName)
          .NotEmpty().WithMessage("First Name is required")
          .Length(3, 15).WithMessage("First Name must be between 3 and 15 characters");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name is required")
                .Length(3, 15).WithMessage("Last Name must be between 3 and 15 characters");

            RuleFor(x => x.Position)
                .NotEmpty().WithMessage("Position is required")
                .Length(3, 20).WithMessage("Position must be between 3 and 20 characters");
        }
    }
}
