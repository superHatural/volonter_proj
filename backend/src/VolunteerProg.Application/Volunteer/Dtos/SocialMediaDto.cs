namespace VolunteerProg.Application.Volunteer.Dtos;

public record SocialMediaDto(string Title, string Link)
{
    public string Title { get; set; } = Title;
    public string Link { get; set; } = Link;
}