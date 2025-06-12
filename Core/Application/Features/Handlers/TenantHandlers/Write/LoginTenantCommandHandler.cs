using Application.Features.Commands.TenantCommands;
using Application.Features.Results.TenantResults;
using Application.Repositories.Master;
using MediatR;

namespace Application.Features.Handlers.TenantHandlers.Write;

public class LoginTenantCommandHandler : IRequestHandler<LoginTenantCommand,LoginTenantUserQueryResult>
{
    private readonly ITenantRepository  _tenantRepository;

    public LoginTenantCommandHandler(ITenantRepository tenantRepository)
    {
         _tenantRepository = tenantRepository;
    }
    public async Task<LoginTenantUserQueryResult> Handle(LoginTenantCommand request, CancellationToken cancellationToken)
    {
        var tenant = await _tenantRepository.GetTenantByCompanyAndEmailAsync(request.Email, request.Password);

        if (tenant == null)
            throw new Exception("Şirket bilgileri bulunamadı.");

        return new LoginTenantUserQueryResult
        {
            Id = tenant.Id,
            CompanyName = tenant.CompanyName,
            ConnectionString = tenant.ConnectionString,
            Email = tenant.Email,
        };
    }
}