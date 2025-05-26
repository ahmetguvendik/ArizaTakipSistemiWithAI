using Application.Features.Commands.AppUserCommands;
using Application.ViewModels;
using Azure.Core;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ModelContextProtocol.NET.Core.Models.Protocol.Base;

namespace Application.Features.Handlers.AppUserHandlers.Write;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
{
    private readonly UserManager<AppUser>  _userManager;

    public ResetPasswordCommandHandler(UserManager<AppUser> userManager)
    {
         _userManager = userManager;
    }
    
    public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            throw new Exception("Kullanıcı bulunamadı.");
        }

        // 2. Şifre sıfırlama işlemi
        var resetResult = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);

        if (!resetResult.Succeeded)
        {
            // Hataları birleştirip fırlatabiliriz
            var errors = string.Join(", ", resetResult.Errors.Select(e => e.Description));
            throw new Exception($"Şifre sıfırlama başarısız: {errors}");
        }
        
    }
}