using Application.Features.Queries.AppUserQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WebApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class UserController  : Microsoft.AspNetCore.Mvc.Controller
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
         _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()  
    {
        try
        {
            var valus = await _mediator.Send(new GetTeknisyenUserQuery());
            return Ok(valus);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error in UserController.Get");
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpGet("GetUserById/{id}")]   
    public async Task<IActionResult> GetUserById(string id)
    {
        try
        {
            var valus = await _mediator.Send(new GetUserByIdQuery(id));
            return Ok(valus);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error in UserController.GetUserById (id: {Id})", id);
            return StatusCode(500, "Internal server error");
        }
    }
}