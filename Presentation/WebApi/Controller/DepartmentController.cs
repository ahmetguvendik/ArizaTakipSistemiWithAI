using Application.Features.Queries;
using Application.Features.Results.DepartmentResults;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    [Authorize]
    public async Task<IActionResult> Get(string id)
    {
        var values = await _mediator.Send(new GetDepartmentByUserIdQuery(id));
        return Ok(values);  
    }
    
    [HttpGet("GetAll")]
    [Authorize]
    public async Task<IActionResult> GetAll()   
    {
        var values = await _mediator.Send(new GetAllDepartmentQuery());
        return Ok(values);  
    }
}