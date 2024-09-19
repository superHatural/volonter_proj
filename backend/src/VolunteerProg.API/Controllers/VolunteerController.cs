using Microsoft.AspNetCore.Mvc;
using VolunteerProg.API.Extentions;
using VolunteerProg.Application.Volunteer.CreateVolunteer.Handlers;
using VolunteerProg.Application.Volunteer.CreateVolunteer.Requests;
using VolunteerProg.Application.Volunteer.Dtos;
using VolunteerProg.Application.Volunteer.UpdateVolunteer.UpdateMainInfo.Handler;
using VolunteerProg.Application.Volunteer.UpdateVolunteer.UpdateMainInfo.Request;
using VolunteerProg.Application.Volunteer.UpdateVolunteer.UpdateRequisites.Handlers;
using VolunteerProg.Application.Volunteer.UpdateVolunteer.UpdateRequisites.Requests;
using VolunteerProg.Application.Volunteer.UpdateVolunteer.UpdateSocialMedia.Handlers;
using VolunteerProg.Application.Volunteer.UpdateVolunteer.UpdateSocialMedia.Requests;

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
        [FromBody]  UpdateVolunteerRequisitesDto dto,
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
}