using Application.Features.Queries.StatisticsQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class StatisticsController : Microsoft.AspNetCore.Mvc.Controller
{
    private readonly IMediator _mediator;

    public StatisticsController(IMediator mediator)
    {
         _mediator = mediator;
    }
    
    [HttpGet("GetAssignedlFaultCount")]
    public async Task<IActionResult> GetAssignedlFaultCount()   
    {
        var valus = await _mediator.Send(new GetAssignedlFaultCountQuery());
        return Ok(valus);
    }
    
    [HttpGet("GetTotalFaultCount")]
    public async Task<IActionResult> GetTotalFaultCount()   
    {
        var valus = await _mediator.Send(new GetTotalFaultCountQuery());    
        return Ok(valus);
    }
    
    [HttpGet("GetClosedFaultCount")]
    public async Task<IActionResult> GetClosedFaultCount()   
    {
        var valus = await _mediator.Send(new GetClosedFaultCountQuery());
        return Ok(valus);
    }
    
    [HttpGet("GetNewFaultCount")]
    public async Task<IActionResult> GetNewFaultCount()   
    {
        var valus = await _mediator.Send(new GetNewFaultCountQuery());
        return Ok(valus);
    }
    
    [HttpGet("GetAverageAssignmentTimeInMinutes")]
    public async Task<IActionResult> GetAverageAssignmentTimeInMinutes()   
    {
        var valus = await _mediator.Send(new GetTimeFaultAssignedToTeknosyenCountQuery());  
        return Ok(valus);
    }
    
    [HttpGet("GetAverageClosedTimeInMinutes")]
    public async Task<IActionResult> GetAverageClosedTimeInMinutes()   
    {
        var valus = await _mediator.Send(new GetAverageClosedTimeInMinutesQuery());  
        return Ok(valus);
    }

    
    
}   