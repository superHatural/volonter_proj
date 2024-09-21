namespace VolunteerProg.Domain.Shared;

public interface ISoftDelete
{
    public void Delete();
    public void Restore();
}