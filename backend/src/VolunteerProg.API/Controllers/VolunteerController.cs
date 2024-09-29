using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using VolunteerProg.API.Contracts;
using VolunteerProg.API.Extentions;
using VolunteerProg.API.Processors;
using VolunteerProg.Application.Volunteer.Create.Handlers;
using VolunteerProg.Application.Volunteer.Create.Requests;
using VolunteerProg.Application.Volunteer.Delete.Handlers;
using VolunteerProg.Application.Volunteer.Delete.Requests;
using VolunteerProg.Application.Volunteer.Dtos;
using VolunteerProg.Application.Volunteer.PetCreate.AddFile.AddFileHandler;
using VolunteerProg.Application.Volunteer.PetCreate.AddFile.AddFileRequest;
using VolunteerProg.Application.Volunteer.PetCreate.Create.Handler;
using VolunteerProg.Application.Volunteer.PetCreate.Create.Requests;
using VolunteerProg.Application.Volunteer.PetCreate.GetFiles.GetFilesHandler;
using VolunteerProg.Application.Volunteer.Update.UpdateMainInfo.Handler;
using VolunteerProg.Application.Volunteer.Update.UpdateMainInfo.Request;
using VolunteerProg.Application.Volunteer.Update.UpdateRequisites.Handlers;
using VolunteerProg.Application.Volunteer.Update.UpdateRequisites.Requests;
using VolunteerProg.Application.Volunteer.Update.UpdateSocialMedia.Handlers;
using VolunteerProg.Application.Volunteer.Update.UpdateSocialMedia.Requests;
using VolunteerProg.Domain.Shared;

namespace VolunteerProg.API.Controllers;

public class VolunteerController : ApplicationController
{
    [HttpGet("{id:guid}")]
    public IActionResult GetById([FromRoute] Guid id)
    {
        return Ok(id);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] CreateVolunteerHandler handler,
        [FromBody] CreateVolunteerRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        return Ok(result.Value);
    }

    [HttpPut("{id:guid}/main-info")]
    public async Task<ActionResult<Guid>> UpdateMainInfo([FromRoute] Guid id,
        [FromServices] UpdateVolunteerMainInfoHandler handler,
        [FromBody] UpdateVolunteerMainInfoDto dto,
        CancellationToken cancellationToken)
    {
        var request = new UpdateVolunteerMainInfoRequest(id, dto);
        var result = await handler.Handle(request, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        return Ok(result.Value);
    }

    [HttpPut("{id:guid}/requisites")]
    public async Task<ActionResult<Guid>> UpdateRequisiteInfo([FromRoute] Guid id,
        [FromServices] UpdateVolunteerRequisitesHandler handler,
        [FromBody] UpdateVolunteerRequisitesDto dto,
        CancellationToken cancellationToken)
    {
        var request = new UpdateVolunteerRequisitesRequest(id, dto);
        var result = await handler.Handle(request, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        return Ok(result.Value);
    }

    [HttpPut("{id:guid}/social-medias")]
    public async Task<ActionResult<Guid>> UpdateSocialMediaInfo([FromRoute] Guid id,
        [FromServices] UpdateVolunteerSocialMediaHandler handler,
        [FromBody] UpdateVolunteerSocialMediaDto dto,
        CancellationToken cancellationToken)
    {
        var request = new UpdateVolunteerSocialMediaRequest(id, dto);
        var result = await handler.Handle(request, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        return Ok(result.Value);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Guid>> Delete([FromRoute] Guid id,
        [FromServices] DeleteVolunteerHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new DeleteVolunteerRequest(id);
        var result = await handler.Handle(request, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        return Ok(result.Value);
    }

    [HttpPost("{id:guid}/pet")]
    public async Task<ActionResult> AddPet(
        [FromRoute] Guid id,
        [FromBody] CreatePetRequest request,
        [FromServices] IValidator<CreatePetCommand> validator,
        [FromServices] CreatePetHandler handler,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        
        var resultValidation = await validator.ValidateAsync(command, cancellationToken);
        if (!resultValidation.IsValid)
            return BadRequest(resultValidation.Errors);
        
        var result = await handler.Handle(command, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();
        return Ok(result.Value);
    }

    [HttpPost("{volunteerId:guid}/pet-files/{petId:guid}")]
    public async Task<ActionResult> AddFilePet(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromForm] IFormFileCollection files,
        [FromServices] AddFileHandler handler,
        CancellationToken cancellationToken)
    {
        await using var proc = new FormFileProcessor();
        var filesDto = proc.Process(files);
        var request = new AddFileRequest(filesDto, volunteerId, petId);
        var result = await handler.Handle(request, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();
        return Ok(result.Value);
    }
}