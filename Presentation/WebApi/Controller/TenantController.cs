using Application.Features.Commands.TenantCommands;
using Application.Features.Results.TenantResults;
using Application.Repositories.Master;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller;

[ApiController]
[Route("api/[controller]")]
public class TenantController : ControllerBase
{
    private readonly IMediator _mediator;

    public TenantController(IMediator mediator)
    {
         _mediator = mediator;
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> LoginTenant([FromBody] LoginTenantCommand command)
    {
        try
        {
            LoginTenantUserQueryResult result = await _mediator.Send(command);
           // HttpContext.Session.SetString("TenantCS", result.ConnectionString);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                message = "Login failed",
                error = ex.Message
            });
        }
    }
    
}