using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using VolunterProg.API.Extentions;
using VolunterProg.Application.Voluunter;
using VolunterProg.Application.Voluunter.UpdateModule;

namespace VolunterProg.API.Controllers;


[ApiController]
[Route("[controller]")]
public class VoluunterController:ControllerBase
{

    [HttpGet("{id:guid}")]
    public IActionResult GetById([FromRoute]Guid id)
    {
        return Ok(id);
    }

    [HttpPost]
    public async Task <ActionResult<Guid>> Create(
        [FromServices] CreateVoluunterHandler handler,
        [FromBody]CreateVoluunterRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        if(result.IsFailure)
            return result.Error.ToResponse(); 
        return Ok(result.Value);
    }
    
    [HttpPut("{id:guid}")]
    public IActionResult Update([FromRoute] Guid id, [FromBody]VoluunterDto dto)
    {
        var request = new UpdateVoluunterRequest(id, dto);
        return Ok(request);
    }
}