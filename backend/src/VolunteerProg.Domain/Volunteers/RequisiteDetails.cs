using VolunteerProg.Domain.ValueObjects;

namespace VolunteerProg.Domain.Volunteers;

public record RequisiteDetails
{
    public IReadOnlyList<Requisite> Requisites;

    private RequisiteDetails()
    {
    }

    public RequisiteDetails(IEnumerable<Requisite> requisites)
    {
        Requisites = requisites.ToList();
    }
}