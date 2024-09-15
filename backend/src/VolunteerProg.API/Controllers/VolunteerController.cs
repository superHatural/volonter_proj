using Microsoft.AspNetCore.Mvc;
using VolunteerProg.API.Extentions;
using VolunteerProg.API.Response;
using VolunteerProg.Application.Volunteer.CreateVolunteer;
using VolunteerProg.Application.Volunteer.Dtos;
using VolunteerProg.Application.Volunteer.UpdateModule;

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

    [HttpPut("{id:guid}")]
    public IActionResult Update([FromRoute] Guid id, [FromBody] VolunteerDto dto)
    {
        var request = new UpdateVolunteerRequest(id, dto);
        return Ok(request);
    }
}