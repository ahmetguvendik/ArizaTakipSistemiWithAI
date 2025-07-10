using Application.Features.Queries.StatisticsQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

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
        try
        {
            var valus = await _mediator.Send(new GetAssignedlFaultCountQuery());
            return Ok(valus);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error in StatisticsController.GetAssignedlFaultCount");
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpGet("GetTotalFaultCount")]
    public async Task<IActionResult> GetTotalFaultCount()   
    {
        try
        {
            var valus = await _mediator.Send(new GetTotalFaultCountQuery());    
            return Ok(valus);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error in StatisticsController.GetTotalFaultCount");
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpGet("GetClosedFaultCount")]
    public async Task<IActionResult> GetClosedFaultCount()   
    {
        try
        {
            var valus = await _mediator.Send(new GetClosedFaultCountQuery());
            return Ok(valus);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error in StatisticsController.GetClosedFaultCount");
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpGet("GetNewFaultCount")]
    public async Task<IActionResult> GetNewFaultCount()   
    {
        try
        {
            var valus = await _mediator.Send(new GetNewFaultCountQuery());
            return Ok(valus);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error in StatisticsController.GetNewFaultCount");
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpGet("GetAverageAssignmentTimeInMinutes")]
    public async Task<IActionResult> GetAverageAssignmentTimeInMinutes()   
    {
        try
        {
            var valus = await _mediator.Send(new GetTimeFaultAssignedToTeknosyenCountQuery());  
            return Ok(valus);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error in StatisticsController.GetAverageAssignmentTimeInMinutes");
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpGet("GetAverageClosedTimeInMinutes")]
    public async Task<IActionResult> GetAverageClosedTimeInMinutes()   
    {
        try
        {
            var valus = await _mediator.Send(new GetAverageClosedTimeInMinutesQuery());  
            return Ok(valus);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error in StatisticsController.GetAverageClosedTimeInMinutes");
            return StatusCode(500, "Internal server error");
        }
    }

    
    
}   