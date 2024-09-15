namespace VolunteerProg.Application.Volunteer.Dtos;

public record RequisiteDto(string Title, string Description)
{
    public string Title { get; set; } = Title;
    public string Description { get; set; } = Description;
}