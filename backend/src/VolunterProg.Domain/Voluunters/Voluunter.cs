using CSharpFunctionalExtensions;
using VolunterProg.Domain.Shared;

namespace VolunterProg.Domain.Voluunters;

public class Voluunter : Shared.Entity<VoluunterId>
{
    public Voluunter(VoluunterId id) : base(id)
    {
    }

    public Voluunter(VoluunterId id,
        FullName fullName,
        Email emailAddress,
        NotEmptyVo description,
        int experience,
        Phone phoneNumber,
        VoluunterDetails? details)
        : base(id)
    {
        Id = id;
        FullName = fullName;
        Email = emailAddress;
        Description = description;
        Experience = experience;
        PhoneNumber = phoneNumber;
        Details = details!;
    }

    private readonly List<Pet> _pets = [];

    public VoluunterId Id { get; private set; }
    public FullName FullName { get; private set; } = default!;
    public Email Email { get; private set; } = default!;
    public NotEmptyVo Description { get; private set; } = default!;
    public int Experience { get; private set; } = default!;
    public Phone PhoneNumber { get; private set; } = default!;
    public VoluunterDetails Details { get; private set; }
    public IReadOnlyList<Pet> Pets => _pets;
    public int NumberOfFoundAHome => FindNumberOfStatus(PetStatus.FoundAHome);
    public int NumberOfNeedsHelp => FindNumberOfStatus(PetStatus.NeedsHelp);
    public int NumberOfLookingForAHome => FindNumberOfStatus(PetStatus.LookingForAHome);

    private int FindNumberOfStatus(PetStatus petStatus)
    {
        var value = from p in Pets
            where p.Status == petStatus
            select p;
        return value.Count();
    }

    public static Result<Voluunter, Error> Create(VoluunterId id,
        FullName fullName,
        Email emailAddress,
        NotEmptyVo description,
        int experience,
        Phone phoneNumber,
        VoluunterDetails? details
    )
    {
        return new Voluunter(id, fullName, emailAddress, description, experience, phoneNumber, details);
    }
}