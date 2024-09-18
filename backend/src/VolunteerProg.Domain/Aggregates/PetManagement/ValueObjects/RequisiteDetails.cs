namespace VolunteerProg.Domain.PetManagement.ValueObjects;

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