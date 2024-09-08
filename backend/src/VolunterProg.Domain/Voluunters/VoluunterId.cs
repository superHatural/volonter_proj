namespace VolunterProg.Domain.Voluunters;

public record VoluunterId
{
    private VoluunterId(Guid value) 
    {
        Value = value;
    }
    public Guid Value { get; }
    public static VoluunterId NewValuunterId() => new(Guid.NewGuid());
    public static VoluunterId Empty() => new(Guid.Empty);
    public static VoluunterId Create(Guid id) => new VoluunterId(id);
}