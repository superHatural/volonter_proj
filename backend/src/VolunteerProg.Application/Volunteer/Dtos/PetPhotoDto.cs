namespace VolunteerProg.Application.Volunteer.Dtos;

public record PetPhotoDto(Stream Stream, string FileName, string ContentType, bool IsMainImage);