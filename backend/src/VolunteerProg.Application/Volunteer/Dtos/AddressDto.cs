using FluentValidation;

namespace VolunteerProg.Application.Volunteer.Dtos;

public record AddressDto(
    string City,
    string Country,
    string PostalCode,
    string Street
);

public class AddressDtoValidation : AbstractValidator<AddressDto>
{
    public AddressDtoValidation()
    {
        RuleFor(x => x.City).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Country).NotEmpty().MaximumLength(50);
        RuleFor(x => x.PostalCode).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Street).NotEmpty().MaximumLength(50);
        
    }
}