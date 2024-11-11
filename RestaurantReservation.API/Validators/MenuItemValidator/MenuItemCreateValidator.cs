using FluentValidation;
using RestaurantReservation.API.DTO_s.MenuItenDto;

namespace RestaurantReservation.API.Validators.MenuItemValidator
{
    public class MenuItemCreateValidator : AbstractValidator<MenuItemCreateDto>
    {
        public MenuItemCreateValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(3, 50).WithMessage("Name must be between 3 and 50 characters");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .Length(10, 200).WithMessage("Description must be between 10 and 200 characters");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero")
                .ScalePrecision(2, 18).WithMessage("Price can have a maximum of 2 decimal places and up to 18 digits in total.");
        }
    }
}
