using CSharpFunctionalExtensions;
using VolunteerProg.Domain.Shared;

namespace VolunteerProg.Domain.Aggregates.PetManagement.ValueObjects;

public record SerialNumber
{
    private SerialNumber(int value)
    {
        Value = value;
    }

    public int Value { get; private set; }

    public static Result<SerialNumber, Error> Create(int number)
    {
        if (number <= 0)
            return Errors.General.ValueIsInvalid("serial number");

        return new SerialNumber(number);
    }
}