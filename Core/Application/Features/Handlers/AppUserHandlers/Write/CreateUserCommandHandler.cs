using Application.Features.Commands.AppUserCommands;
using Application.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace Application.Features.Handlers.AppUserHandlers.Write;

 public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
  
    public CreateUserCommandHandler(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }
    public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var totalUserCount = _userManager.Users.Count();

        if (totalUserCount >= 2)
        {
            throw new Exception("Sistemde zaten 2 veya daha fazla kullanıcı var. Yeni kullanıcı eklenemez.");
        }
            var appUser = new AppUser();
            appUser.UserName = request.Username;    
            appUser.NameSurname = request.NameSurname;
            appUser.DepartmentId = request.DepartmanId;
            appUser.Email = request.Email;
            
            var response = await _userManager.CreateAsync(appUser, request.Password);
            if (response.Succeeded)
            {
                var role = await _roleManager.FindByNameAsync(request.Role);
                if (role == null)
                {
                    var appRole = new AppRole()
                    {
                        Name = "Teknisyen",
                    };
                    await _roleManager.CreateAsync(appRole);
                    await _userManager.AddToRoleAsync(appUser, "Teknisyen");    

                }

                await _userManager.AddToRoleAsync(appUser, request.Role);    
            }
            else
            {
                // Hataları logla
                var errorMessages = string.Join(" | ", response.Errors.Select(e => e.Description));
                Log.Error("Kullanıcı oluşturulamadı: {Errors}", errorMessages);
                // Kullanıcıya sade bir hata fırlat
                throw new Exception("Kullanıcı oluşturulamadı. Lütfen girdiğiniz bilgileri kontrol edin.");
            }
        
    }
}