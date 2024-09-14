using CSharpFunctionalExtensions;
using VolunteerProg.Domain.Shared;

namespace VolunteerProg.Domain.Volunteers;

public class Volunteer : Shared.Entity<VolunteerId>
{
    public Volunteer(VolunteerId id) : base(id)
    {
    }

    public Volunteer(VolunteerId id,
        FullName fullName,
        Email emailAddress,
        NotEmptyVo description,
        int experience,
        Phone phoneNumber,
        VolunteerDetails? details)
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

    public VolunteerId Id { get; private set; }
    public FullName FullName { get; private set; } = default!;
    public Email Email { get; private set; } = default!;
    public NotEmptyVo Description { get; private set; } = default!;
    public int Experience { get; private set; } = default!;
    public Phone PhoneNumber { get; private set; } = default!;
    public VolunteerDetails Details { get; private set; }
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

    public static Result<Volunteer, Error> Create(VolunteerId id,
        FullName fullName,
        Email emailAddress,
        NotEmptyVo description,
        int experience,
        Phone phoneNumber,
        VolunteerDetails? details
    )
    {
        return new Volunteer(id, fullName, emailAddress, description, experience, phoneNumber, details);
    }
}