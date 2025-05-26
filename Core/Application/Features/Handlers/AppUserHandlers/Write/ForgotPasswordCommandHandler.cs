using Application.Features.Commands.AppUserCommands;
using Application.Services;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Handlers.AppUserHandlers.Write;
 
public class ForgotPasswordCommandHandler  : IRequestHandler<ForgotPasswordCommand>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IEmailService  _emailService;

    public ForgotPasswordCommandHandler(UserManager<AppUser> userManager, IEmailService emailService)
    {
         _userManager = userManager;
         _emailService = emailService;
         
    }
    
    public async Task Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
       var user =await _userManager.FindByEmailAsync(request.Email);
       if (user != null)
       {
           var token = await _userManager.GeneratePasswordResetTokenAsync(user);
           var encodedToken = System.Web.HttpUtility.UrlEncode(token);
           var resetLink = $"http://localhost:5297/Login/ResetPassword?email={user.Email}&token={encodedToken}";   
           await _emailService.SendResetPasswordAsync(emailAdress: user.Email, $"Şifre Sıfırlama Talebi Şifrenizi sıfırlamak için lütfen şu linke tıklayın: <a href='{resetLink}'>Şifre Sıfırlama</a>");
       }
    }
}