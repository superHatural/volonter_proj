using VolunteerProg.Application.FileProvider;

namespace VolunteerProg.Application.Volunteer.PetCreate.AddFile;

public record AddFileCommand (IEnumerable<CreateFileData> Files, Guid VolunteerId, Guid PetId);