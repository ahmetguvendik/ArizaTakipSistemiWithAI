using Application.Features.Commands.AppUserCommands;
using Application.Features.Commands.TenantCommands;
using Application.Features.Results.TenantResults;
using Application.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace Application.Features.Handlers.TenantHandlers.Write
{
    public class LoginTenantCommandHandler : IRequestHandler<LoginTenantCommand, LoginTenantUserQueryResult>
    {
        private readonly ITenantRepository _tenantRepository;

        public LoginTenantCommandHandler(ITenantRepository tenantRepository)
        {
             _tenantRepository = tenantRepository;
        }
        
        public async Task<LoginTenantUserQueryResult> Handle(LoginTenantCommand request, CancellationToken cancellationToken)
        {
            var values =  _tenantRepository.GetConnectionString(request.ConnectionString);
            return new LoginTenantUserQueryResult()
            {
                Id = values.Id,
                CompanyName = values.CompanyName,
                Email = values.Email,
                ConnectionString = values.ConnectionString
            };
        }
    }
}