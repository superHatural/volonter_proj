using Microsoft.AspNetCore.Mvc;
using VolunteerProg.API.Extentions;
using VolunteerProg.Application.Volunteer.PetCreate.AddFile.AddFileHandler;
using VolunteerProg.Application.Volunteer.PetCreate.AddFile.AddFileRequest;
using VolunteerProg.Application.Volunteer.PetCreate.DeleteFile.DeleteFileHandler;
using VolunteerProg.Application.Volunteer.PetCreate.GetFiles.GetFilesHandler;
using VolunteerProg.Domain.Shared;

namespace VolunteerProg.API.Controllers;

public class FileController: ApplicationController
{
    [HttpPost("/Files")]
    public async Task<IActionResult> CreateFile(IFormFile? file,
        [FromServices] AddFileHandler handler,
        CancellationToken cancellationToken)
    {
        if (file == null)
        {
            var error = Errors.General.ValueIsRequired("File");
            return error.ToResponse();
        }
        await using var stream = file.OpenReadStream();
        var request = new AddFileRequest(stream, "photos", Guid.NewGuid().ToString());
        var result = await handler.Handle(request, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        return Ok(result.Value);
    }
    
    [HttpPost("/FilesLink")]
    public async Task<IActionResult> GetFiles(List<string> filesNames,
        [FromServices] GetFilesHandler handler)
    {
        var result = await handler.Handle(filesNames);
        if (result.IsFailure)
            return result.Error.ToResponse();
        return Ok(result.Value);
    }
    [HttpDelete("/Files")]
    public async Task<IActionResult> DeleteFile(string fileName,
        [FromServices] DeleteFileHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(fileName, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        return Ok(result.Value);
    }
}