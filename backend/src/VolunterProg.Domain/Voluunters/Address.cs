using CSharpFunctionalExtensions;

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
    
    public static Result<Address> Create(string city, string country, string postalCode, string street)
    {
        if (string.IsNullOrEmpty(city))
            return Result.Failure<Address>($"City is required.");
        if (string.IsNullOrEmpty(country))
            return Result.Failure<Address>($"Country is required.");
        if (string.IsNullOrEmpty(postalCode))
            return Result.Failure<Address>($"Postal Code is required.");
        if (string.IsNullOrEmpty(street))
            return Result.Failure<Address>($"Street is required.");
        return Result.Success(new Address(city, country, postalCode, street));
    }
    
}