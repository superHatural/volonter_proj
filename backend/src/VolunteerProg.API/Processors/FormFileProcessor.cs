using VolunteerProg.Application.FileProvider;

namespace VolunteerProg.API.Processors;

public class FormFileProcessor : IAsyncDisposable
{
    private readonly List<CreateFileData> _filesDto = [];
    public List<CreateFileData> Process(IFormFileCollection files)
    {
        foreach (var file in files)
        {
            var stream = file.OpenReadStream();
            var fileDto = new CreateFileData(stream, file.FileName);
            _filesDto.Add(fileDto);
        }
        return _filesDto;
    }
    public async ValueTask DisposeAsync()
    {
        foreach (var file in _filesDto)
        {
            await file.Stream.DisposeAsync();
        }
    }
}