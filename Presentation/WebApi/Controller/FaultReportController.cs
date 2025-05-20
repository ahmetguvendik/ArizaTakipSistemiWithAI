using Application.Features.Commands.FaultReportComamnds;
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
    
    [HttpPost]
    public async Task<IActionResult> Post(CreateFaultReportCommand command)
    {
        await _mediator.Send(command);
        return Ok("EKlendi");
    }
}
