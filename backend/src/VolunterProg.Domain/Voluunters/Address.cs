using CSharpFunctionalExtensions;

namespace VolunterProg.Domain.Voluunters;

public record Address
{
    public string City { get; private set; } = default!;
    public string Country { get; private set; } = default!;

    private Address(string city, string country)
    {
        City = city;
        Country = country;
    }
    
    public static Result<Address> Create(string city, string country)
    {
        if (string.IsNullOrEmpty(city))
            return Result.Failure<Address>($"City is required.");
        if (string.IsNullOrEmpty(country))
            return Result.Failure<Address>($"Couytri is required.");
        var address = new Address(city, country);
        return Result.Success(address);
    }
    
}