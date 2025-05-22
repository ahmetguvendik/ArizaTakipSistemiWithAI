using Application.Features.Commands.FaultReportComamnds;
using Application.Features.Queries.FaultReportQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class FaultReportController  : Microsoft.AspNetCore.Mvc.Controller
{
    private readonly IMediator _mediator;

    public FaultReportController(IMediator mediator)
    {
         _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var valus = await _mediator.Send(new GetFaultReportQuery());
        return Ok(valus);
    }
    
    [HttpGet("{id}")]       
    public async Task<IActionResult> GetById(string id)
    {
        var valus = await _mediator.Send(new GetFaultReportByIdQuery(id));
        return Ok(valus);
    }
    
    [HttpGet("GetByDepartmanId/{id}")]         
    public async Task<IActionResult> GetByDepartmanId(string id)
    {
        var valus = await _mediator.Send(new GetFaultReportByDepartmanIdQuery(id));
        return Ok(valus);
    }
    
    [HttpPost]
    public async Task<IActionResult> Post(CreateFaultReportCommand command)
    {
        await _mediator.Send(command);
        return Ok("EKlendi");
    }
    
    [HttpPut]
    public async Task<IActionResult> Post(AssignTechnicianCommand command)
    {
        await _mediator.Send(command);
        return Ok("Atandı");
    }
}
