using Microsoft.AspNetCore.Mvc;
using VolunteerProg.Application.Voluunter.CreateVoluunter;
using VolunteerProg.Application.Voluunter.Dtos;
using VolunteerProg.Application.Voluunter.UpdateModule;
using VolunteerProg.API.Extentions;

namespace VolunteerProg.API.Controllers;


[ApiController]
[Route("[controller]")]
public class VolunteerController:ControllerBase
{

    [HttpGet("{id:guid}")]
    public IActionResult GetById([FromRoute]Guid id)
    {
        return Ok(id);
    }

    [HttpPost]
    public async Task <ActionResult<Guid>> Create(
        [FromServices] CreateVolunteerHandler handler,
        [FromBody]CreateVolunteerRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        if(result.IsFailure)
            return result.Error.ToResponse(); 
        return Ok(result.Value);
    }
    
    [HttpPut("{id:guid}")]
    public IActionResult Update([FromRoute] Guid id, [FromBody]VolunteerDto dto)
    {
        var request = new UpdateVolunteerRequest(id, dto);
        return Ok(request);
    }
}