using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Features.Commands.AppUserCommands;

public class ResetPasswordCommand : IRequest
{
    public string Password { get; set; }

    [Compare("Password", ErrorMessage = "Sifreler Eslesmiyor")]
    public string ConfirmPassword { get; set; }

    public string Email { get; set; }

    public string Token { get; set; }
}