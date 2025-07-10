using Application.Features.Queries;
using Application.Features.Results.DepartmentResults;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WebApi.Controller;

[ApiController]
[Route("api/[controller]")]
public class DepartmentController : ControllerBase
{
    private readonly IMediator _mediator;

    public DepartmentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get(string id)
    {
        try
        {
            var values = await _mediator.Send(new GetDepartmentByUserIdQuery(id));
            return Ok(values);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error in DepartmentController.Get (id: {Id})", id);
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()   
    {
        try
        {
            var values = await _mediator.Send(new GetAllDepartmentQuery());
            return Ok(values);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error in DepartmentController.GetAll");
            return StatusCode(500, "Internal server error");
        }
    }
}