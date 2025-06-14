using Application.Features.Results.TenantResults;
using MediatR;

namespace Application.Features.Commands.TenantCommands;

public class LoginTenantCommand : IRequest<LoginTenantUserQueryResult>
{
    public string ConnectionString { get; set; }
}