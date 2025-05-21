using Application.Features.Commands.AppUserCommands;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller;


[Route("api/[controller]")]
[ApiController]
public class RegisterController : Microsoft.AspNetCore.Mvc.Controller
{
    private readonly IMediator  _mediator;

    public RegisterController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost]
    public async Task<IActionResult> Post(CreateUserCommand command)
    {
        await _mediator.Send(command);
        return Ok("EKlendi");   
    }
}