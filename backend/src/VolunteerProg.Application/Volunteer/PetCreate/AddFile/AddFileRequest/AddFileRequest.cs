using VolunteerProg.Application.FileProvider;

namespace VolunteerProg.Application.Volunteer.PetCreate.AddFile.AddFileRequest;

public record AddFileRequest(List<CreateFileData> Files, Guid VolunteerId , Guid PetId);
