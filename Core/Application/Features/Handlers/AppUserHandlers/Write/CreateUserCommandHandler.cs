using Application.Features.Commands.AppUserCommands;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

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
        {
            var appUser = new AppUser();
            appUser.UserName = request.Username;    
            appUser.NameSurname = request.NameSurname;
            appUser.DepartmentId = request.DepartmanId;
            var response = await _userManager.CreateAsync(appUser, request.Password);
            if (response.Succeeded)
            {
                var role = await _roleManager.FindByNameAsync("Teknisyen");
                if (role == null)
                {
                    var appRole = new AppRole()
                    {
                        Name = "Teknisyen",
                    };
                    await _roleManager.CreateAsync(appRole);
                }

                await _userManager.AddToRoleAsync(appUser, "Teknisyen");    
            }
                
        }
    }
}