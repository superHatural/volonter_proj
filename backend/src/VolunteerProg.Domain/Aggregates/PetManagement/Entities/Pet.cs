using System.Globalization;
using System.Runtime.InteropServices.JavaScript;
using CSharpFunctionalExtensions;
using VolunteerProg.Domain.Aggregates.PetManagement.AggregateRoot;
using VolunteerProg.Domain.Aggregates.PetManagement.ValueObjects;
using VolunteerProg.Domain.Shared;
using VolunteerProg.Domain.Shared.Ids;

namespace VolunteerProg.Domain.Aggregates.PetManagement.Entities;

public sealed class Pet : Shared.Entity<PetId>, ISoftDelete
{
    private bool _deleted = false;

    private Pet(PetId id) : base(id)
    {
    }

    public Pet(
        PetId id,
        NotEmptyVo name,
        NotEmptyVo description,
        SpeciesDetails speciesDetails,
        NotEmptyVo color,
        NotEmptyVo healthInfo,
        Address address,
        int weight,
        int height,
        Phone phoneNumber,
        bool isCastrated,
        Date birthDate,
        bool isVaccinated,
        PetStatus status,
        PetPhotoDetails? petPhotoDetails,
        RequisiteDetails? requisiteDetails) : base(id)
    {
        Name = name;
        Description = description;
        SpeciesDetails = speciesDetails;
        Color = color;
        HealthInfo = healthInfo;
        Address = address;
        Weight = weight;
        Height = height;
        PhoneNumber = phoneNumber;
        IsCastrated = isCastrated;
        BirthDate = birthDate;
        IsVaccinated = isVaccinated;
        Status = status;
        PetPhotoDetails = petPhotoDetails;
        RequisiteDetails = requisiteDetails;
        DateOfCreate = Date.Create(DateTime.Now.ToString(CultureInfo.CurrentCulture)).Value;
        
    }
    
    public NotEmptyVo Name { get; private set; } = default!;
    public NotEmptyVo Description { get; private set; } = default!;
    public SpeciesDetails SpeciesDetails { get; private set; } = default!;
    public NotEmptyVo Color { get; private set; } = default!;
    public NotEmptyVo HealthInfo { get; private set; } = default!;
    public Address Address { get; private set; } = default!;
    public int Weight { get; private set; } = default!;
    public int Height { get; private set; } = default!;
    public Phone PhoneNumber { get; private set; } = default!;
    public bool IsCastrated { get; private set; } = default!;
    public Date BirthDate { get; private set; } = default!;
    public bool IsVaccinated { get; private set; } = default!;
    public PetStatus Status { get; private set; } = default!;
    public PetPhotoDetails? PetPhotoDetails { get; private set; }
    public RequisiteDetails? RequisiteDetails { get; private set; }
    public Date DateOfCreate { get; private set; } = default!;

    public void AddPhoto(PetPhotoDetails photoDetails)
    {
        PetPhotoDetails = photoDetails;
    }

    public void Delete()
    {
        if (!_deleted)
            _deleted = true;
    }

    public void Restore()
    {
        if (_deleted)
            _deleted = false;
    }
}