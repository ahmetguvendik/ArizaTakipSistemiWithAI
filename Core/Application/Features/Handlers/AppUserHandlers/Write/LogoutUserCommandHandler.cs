using Application.Features.Commands.AppUserCommands;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Handlers.AppUserHandlers.Write;

public class LogoutUserCommandHandler: IRequestHandler<LogoutUserCommand>
{
    private readonly SignInManager<AppUser> _signInManager;

    public LogoutUserCommandHandler(SignInManager<AppUser> signInManager)
    {
        _signInManager = signInManager;
    }

    public async Task Handle(LogoutUserCommand request, CancellationToken cancellationToken)
    {
        await _signInManager.SignOutAsync();
    }
}