namespace VolunterProg.Domain;

public class Pet
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Breed { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Species { get; set; } = default!;
    public string Color { get; set; } = default!;
    public string HealthInfo{ get; set; } = default!;
    public string Address { get; set; } = default!;
    public string Weight { get; set; } = default!;
    public string Height { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public bool IsCastrated { get; set; } = default!;
    public string BirthDate { get; set; } = default!;
    public bool IsVaccinated { get; set; } = default!;
    public string Status { get; set; } = default!;
    public List<Requisite>? Requisites { get; set; } = [];
    public string DateOfCreate { get; set; } = default!;
}