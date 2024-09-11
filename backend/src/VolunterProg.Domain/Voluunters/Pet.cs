using CSharpFunctionalExtensions;

namespace VolunterProg.Domain.Voluunters;

public class Pet: Shared.Entity<PetId>
{
    public Pet(PetId id) : base(id)
    {
        
    }

    private Pet(PetId id, NotEmptyVo name, NotEmptyVo description):base(id)
    {
        Id = id;
        Name = name;
        Description = description;
    }
    public PetId Id { get; private set; }
    public NotEmptyVo Name { get; private set; } = default!;
    public NotEmptyVo Description { get; private set; } = default!;
    public SpeciesDetails SpeciesDetails { get; private set; } = default!;
    public NotEmptyVo Color { get; private set; } = default!;
    public NotEmptyVo HealthInfo{ get; private set; } = default!;
    public Address Address { get; private set; } = default!;
    public int Weight { get; private set; } = default!;
    public int Height { get; private set; } = default!;
    public Phone PhoneNumber { get; private set; } = default!;
    public bool IsCastrated { get; private set; } = default!;
    public Date BirthDate { get; private set; } = default!;
    public bool IsVaccinated { get; private set; } = default!;
    public PetStatus Status { get; private set; } = default!;
    public PetDetails? Details { get; private set; } 
    public Date DateOfCreate { get; private set; } = default!;
    public static Result<Pet> Create(PetId id,NotEmptyVo name, NotEmptyVo emailAddress)
    {
        return Result.Success(new Pet(id, name, emailAddress));
    }
}