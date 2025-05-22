using Application.Features.Commands.AppUserCommands;
using Application.Features.Results.AppUserResults;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Handlers.AppUserHandlers.Write;

 public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserQueryResult>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        public LoginUserCommandHandler(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<LoginUserQueryResult> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var appUser = await _userManager.FindByNameAsync(request.Username); 
            await _userManager.SetTwoFactorEnabledAsync(appUser, true);
            var response = await _signInManager.PasswordSignInAsync(appUser,request.Password,false,false);
            if (response.Succeeded)
            {
                var role = await _userManager.GetRolesAsync(appUser);
                if (role.Contains("Admin"))
                {
                    return new LoginUserQueryResult()
                    {
                        Id = appUser.Id,
                        Username = request.Username,
                        DepartmanId = appUser.DepartmentId,
                        Role = "Admin",
      
                    };
                }
                else if (role.Contains("Teknisyen"))
                {
                    return new LoginUserQueryResult()
                    {
                        Id = appUser.Id,
                        Username = request.Username,
                        DepartmanId = appUser.DepartmentId,
                        Role = "Teknisyen",
               
                    };
                }
                else if (role.Contains("Supervisor"))
                {
                    return new LoginUserQueryResult()
                    {
                        Id = appUser.Id,
                        Username = request.Username,
                        DepartmanId = appUser.DepartmentId,
                        Role = "Supervisor"
                    };
                }
                
            }
            
            return new LoginUserQueryResult()
            {
                Id = appUser.Id,
                Username = request.Username,
                DepartmanId = appUser.DepartmentId,
                Role = ""
            };
            
        }
    }