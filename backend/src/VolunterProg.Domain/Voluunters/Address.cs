using CSharpFunctionalExtensions;
using VolunterProg.Domain.Shared;

namespace VolunterProg.Domain.Voluunters;

public record Address
{
    public string City { get;  } = default!;
    public string Country { get;  } = default!;
    public string PostalCode { get;  } = default!;
    public string Street{ get;  } = default!;

    private Address(string city, string country, string postalCode, string street)
    {
        City = city;
        Country = country;
        PostalCode = postalCode;
        Street = street;
    }
    
    public static Result<Address, Error> Create(string city, string country, string postalCode, string street)
    {
        if (string.IsNullOrEmpty(city))
            return Errors.General.ValueIsRequired("City");
        if (string.IsNullOrEmpty(country))
            return Errors.General.ValueIsRequired("Country");
        if (string.IsNullOrEmpty(postalCode))
            return Errors.General.ValueIsRequired("PostalCode");
        if (string.IsNullOrEmpty(street))
            return Errors.General.ValueIsRequired("Street");
        return new Address(city, country, postalCode, street);
    }
    
}