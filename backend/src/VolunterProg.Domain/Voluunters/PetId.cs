namespace VolunterProg.Domain.Voluunters;

public record PetId
{
    private PetId(Guid value) 
    {
        Value = value;
    }
    public Guid Value { get; }
    public static PetId NewValuunterId() => new(Guid.NewGuid());
    public static PetId Empty() => new(Guid.Empty);
    public static PetId Create(Guid id) => new PetId(id);
}