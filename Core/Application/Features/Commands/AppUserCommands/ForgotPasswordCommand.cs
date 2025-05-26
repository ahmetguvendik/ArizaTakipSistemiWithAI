using MediatR;

namespace Application.Features.Commands.AppUserCommands;

public class ForgotPasswordCommand : IRequest
{
    public string Email { get; set; }
}