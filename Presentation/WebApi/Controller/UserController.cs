using Application.Features.Queries.AppUserQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        var valus = await _mediator.Send(new GetTeknisyenUserQuery());
        return Ok(valus);
    }
}