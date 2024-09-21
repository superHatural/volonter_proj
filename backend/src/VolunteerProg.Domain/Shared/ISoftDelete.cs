namespace VolunteerProg.Domain.Aggregates.PetManagement.AggregateRoot;

public interface ISoftDelete
{
    public void Delete();
    public void Restore();
}