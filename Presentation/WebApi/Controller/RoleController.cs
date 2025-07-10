using Application.Features.Queries.AppRoleQueries;
using Application.Features.Results.AppRoleResults;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WebApi.Controller;

[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly IMediator _mediator;

    public RoleController(IMediator mediator)
    {
         _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var values = await _mediator.Send(new GetAllRoleQuery());
            return Ok(values);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error in RoleController.GetAll");
            return StatusCode(500, "Internal server error");
        }
    }
}