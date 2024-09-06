namespace VolunterProg.Domain;

public class Pet
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = default!;
    public string Breed { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public string Species { get; private set; } = default!;
    public string Color { get; private set; } = default!;
    public string HealthInfo{ get; private set; } = default!;
    public string Address { get; private set; } = default!;
    public string Weight { get; private set; } = default!;
    public string Height { get; private set; } = default!;
    public string PhoneNumber { get; private set; } = default!;
    public bool IsCastrated { get; private set; } = default!;
    public string BirthDate { get; private set; } = default!;
    public bool IsVaccinated { get; private set; } = default!;
    public string Status { get; private set; } = default!;
    public List<Requisite> Requisites { get; private set; } = [];
    public string DateOfCreate { get; private set; } = default!;
}