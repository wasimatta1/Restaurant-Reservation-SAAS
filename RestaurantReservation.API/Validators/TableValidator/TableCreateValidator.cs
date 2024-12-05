using FluentValidation;
using RestaurantReservation.API.DTO_s.TableDto;

namespace RestaurantReservation.API.Validators.TableValidator
{
    public class TableCreateValidator : AbstractValidator<TableCreateDto>
    {
        public TableCreateValidator() 
        {
            RuleFor(x => x.Capacity)
           .GreaterThan(0).WithMessage("Capacity must be a positive integer")
           .LessThanOrEqualTo(20).WithMessage("Capacity cannot exceed 20");
        }
    }
}
