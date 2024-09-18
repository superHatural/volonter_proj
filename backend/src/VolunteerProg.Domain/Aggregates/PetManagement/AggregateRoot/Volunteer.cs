using CSharpFunctionalExtensions;
using VolunteerProg.Domain.PetManagement.Entities;
using VolunteerProg.Domain.PetManagement.ValueObjects;
using VolunteerProg.Domain.PetManagement.ValueObjects.Ids;
using VolunteerProg.Domain.Shared;

namespace VolunteerProg.Domain.PetManagement.AggregateRoot;

public sealed class Volunteer : Shared.Entity<VolunteerId>
{
    private Volunteer(VolunteerId id) : base(id)
    {
    }

    public Volunteer(VolunteerId id,
        FullName fullName,
        Email emailAddress,
        NotEmptyVo description,
        int experience,
        Phone phoneNumber,
        SocialMediasDetails? socMedDetails,
        RequisiteDetails? reqDetails)
        : base(id)
    {
        FullName = fullName;
        Email = emailAddress;
        Description = description;
        Experience = experience;
        PhoneNumber = phoneNumber;
        SocMedDetails = socMedDetails!;
        ReqDetails = reqDetails!;
    }

    private readonly List<Pet> _pets = [];
    

    public FullName FullName { get; private set; } = default!;
    public Email Email { get; private set; } = default!;
    public NotEmptyVo Description { get; private set; } = default!;
    public int Experience { get; private set; } = default!;
    public Phone PhoneNumber { get; private set; } = default!;
    public SocialMediasDetails SocMedDetails { get; private set; }
    public RequisiteDetails ReqDetails { get; private set; }
    
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
        SocialMediasDetails? socMedDetails,
        RequisiteDetails? reqDetails
    )
    {
        return new Volunteer(id, fullName, emailAddress, description, experience, phoneNumber, socMedDetails, reqDetails);
    }
}