using CSharpFunctionalExtensions;

namespace VolunterProg.Domain.Voluunters;

public class Voluunter: Shared.Entity<VoluunterId>
{
    public Voluunter(VoluunterId id) : base(id)
    {
        
    }

    private Voluunter(VoluunterId id, string fullName, string emailAddress)
    : base(id)
    {
        Id = id;
        FullName = fullName;
        Email = emailAddress;
    }
    private readonly List<Pet> _pets = [];
    
    public VoluunterId Id { get; private set; }
    public string FullName { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public int Experience { get; private set; } = default!;
    public string PhoneNumber { get; private set; } = default!;
    public VoluunterDetails Details { get; private set; }
    public IReadOnlyList<Pet> Pets  => _pets;
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

    public static Result<Voluunter> Create(VoluunterId id,string fullName, string emailAddress)
    {
        if (string.IsNullOrEmpty(fullName))
            return Result.Failure<Voluunter>($"Full name is required.");
        if (string.IsNullOrEmpty(emailAddress))
            return Result.Failure<Voluunter>($"Email is required.");
        return Result.Success(new Voluunter(id, fullName, emailAddress));
    }
    
}