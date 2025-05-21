using Application.Features.Results.AppUserResults;
using MediatR;

namespace Application.Features.Commands.AppUserCommands;

public class LoginUserCommand : IRequest<LoginUserQueryResult>
{
    public string Username { get; set; }
    public string Password { get; set; }
}