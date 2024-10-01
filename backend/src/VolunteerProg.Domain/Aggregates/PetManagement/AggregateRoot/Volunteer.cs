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
        ValueObjectList<SocialMedia>? socMedDetails,
        ValueObjectList<Requisite>? reqDetails)
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
    public ValueObjectList<SocialMedia> SocMedDetails { get; private set; }
    public ValueObjectList<Requisite> ReqDetails { get; private set; }

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
        ValueObjectList<Requisite>? reqDetails)
    {
        ReqDetails = reqDetails!;

        return this;
    }

    public Result<Volunteer, Error> UpdateSocialMediaInfo(
        ValueObjectList<SocialMedia>? socMedDetails)
    {
        SocMedDetails = socMedDetails!;

        return this;
    }

    public UnitResult<Error> AddPet(Pet pet)
    {
        if (_pets.Count == 0)
        {
            var serialNumber = SerialNumber.Create(1).Value;
            pet.SetSerialNumber(serialNumber);
        }
        else
        {
            var serialNumber = SerialNumber.Create(_pets.Count + 1).Value;
            pet.SetSerialNumber(serialNumber);
        }

        _pets.Add(pet);
        return Result.Success<Error>();
    }

    public UnitResult<Error> AddFilePet(PetId petId, ValueObjectList<FilePathData> photoDetails)
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

    public UnitResult<Error> ChangePetPosition(PetId petId, SerialNumber newPosition)
    {
        if (newPosition.Value > _pets.Count || newPosition.Value <= 0)
            return Errors.General.ValueIsInvalid("newPosition");

        var pet = _pets.FirstOrDefault(p => p.Id == petId);
        if (pet == null)
            return Errors.General.NotFound(petId);

        if (pet.SerialNumber == newPosition)
            return Result.Success<Error>();

        var currentPosition = pet.SerialNumber;

        if (newPosition.Value > currentPosition.Value)
        {
            foreach (var otherPet in _pets.Where(p =>
                         p.SerialNumber.Value > currentPosition.Value && p.SerialNumber.Value <= newPosition.Value))
            {
                otherPet.SetSerialNumber(SerialNumber.Create(otherPet.SerialNumber.Value - 1).Value);
            }
        }
        else
        {
            foreach (var otherPet in _pets.Where(p =>
                         p.SerialNumber.Value < currentPosition.Value && p.SerialNumber.Value >= newPosition.Value))
            {
                otherPet.SetSerialNumber(SerialNumber.Create(otherPet.SerialNumber.Value + 1).Value);
            }
        }

        pet.SetSerialNumber(newPosition);

        return Result.Success<Error>();
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