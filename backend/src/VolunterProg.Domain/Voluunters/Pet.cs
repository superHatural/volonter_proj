using CSharpFunctionalExtensions;

namespace VolunterProg.Domain.Voluunters;

public class Pet: Shared.Entity<PetId>
{
    public Pet(PetId id) : base(id)
    {
        
    }

    private Pet(PetId id, string name, string description):base(id)
    {
        Id = id;
        Name = name;
        Description = description;
    }
    public PetId Id { get; private set; }
    public string Name { get; private set; } = default!;
    public string Breed { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public string Species { get; private set; } = default!;
    public string Color { get; private set; } = default!;
    public string HealthInfo{ get; private set; } = default!;
    public Address Address { get; private set; } = default!;
    public string Weight { get; private set; } = default!;
    public string Height { get; private set; } = default!;
    public string PhoneNumber { get; private set; } = default!;
    public bool IsCastrated { get; private set; } = default!;
    public string BirthDate { get; private set; } = default!;
    public bool IsVaccinated { get; private set; } = default!;
    public PetStatus Status { get; private set; } = default!;
    public PetDetails? Details { get; private set; } 
    public string DateOfCreate { get; private set; } = default!;
    public static Result<Pet> Create(PetId id,string fullName, string emailAddress)
    {
        if (string.IsNullOrEmpty(fullName))
            return Result.Failure<Pet>($"Full name is required.");
        if (string.IsNullOrEmpty(emailAddress))
            return Result.Failure<Pet>($"Email is required.");
        var pet = new Pet(id, fullName, emailAddress);
        return Result.Success(pet);
    }
}

public enum PetStatus
{
    NeedsHelp,
    LookingForAHome,
    FoundAHome,
}