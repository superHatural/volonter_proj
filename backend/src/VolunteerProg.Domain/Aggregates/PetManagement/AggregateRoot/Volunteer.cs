using System.Runtime.InteropServices.JavaScript;
using CSharpFunctionalExtensions;
using VolunteerProg.Domain.Aggregates.PetManagement.Entities;
using VolunteerProg.Domain.Aggregates.PetManagement.ValueObjects;
using VolunteerProg.Domain.Shared;
using VolunteerProg.Domain.Shared.Ids;

namespace VolunteerProg.Domain.Aggregates.PetManagement.AggregateRoot;

public sealed class Volunteer : Shared.Entity<VolunteerId>, ISoftDelete
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

    private bool _deleted = false;
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

    public Result<Volunteer, Error> UpdateMainInfo(
        FullName fullName,
        Email emailAddress,
        NotEmptyVo description,
        int experience,
        Phone phoneNumber)
    {
        FullName = fullName;
        Email = emailAddress;
        Description = description;
        Experience = experience;
        PhoneNumber = phoneNumber;

        return this;
    }

    public Result<Volunteer, Error> UpdateRequisiteInfo(
        RequisiteDetails? reqDetails)
    {
        ReqDetails = reqDetails!;

        return this;
    }

    public Result<Volunteer, Error> UpdateSocialMediaInfo(
        SocialMediasDetails? socMedDetails)
    {
        SocMedDetails = socMedDetails!;

        return this;
    }

    public UnitResult<Error> AddPet(Pet pet)
    {
        _pets.Add(pet);
        return Result.Success<Error>();
    }

    public UnitResult<Error> AddFilePet(PetId petId, PetPhotoDetails photoDetails)
    {
        var pet = _pets.FirstOrDefault(p => p.Id == petId);
        if (pet == null)
            return Errors.General.NotFound(petId);
        pet.AddPhoto(photoDetails);
        return Result.Success<Error>();
    }

    public void Delete()
    {
        if (!_deleted)
            _deleted = true;
        foreach (var pet in Pets)
        {
            pet.Delete();
        }
        
    }

    public void Restore()
    {
        if (!_deleted)
            _deleted = false;
        foreach (var pet in Pets)
        {
            pet.Restore();
        }
    }
}