using Application.Features.Commands.MachineCommands;
using Application.Features.Queries.FaultReportQueries;
using Application.Features.Queries.MachineQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class MachineController : Microsoft.AspNetCore.Mvc.Controller
{
    private readonly IMediator _mediator;

    public MachineController(IMediator mediator)
    {
         _mediator = mediator;
    }
    
    [HttpGet("GetMachineByDepartmanId/{id}")]         
    public async Task<IActionResult> GetMachineByDepartmanId(string id)
    {
        var valus = await _mediator.Send(new GetMachineByDepartmanIdQuery(id));
        return Ok(valus);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMachine(CreateMachineCommand command)
    {
        await _mediator.Send(command);
        return Ok("Eklendi");
    }
}