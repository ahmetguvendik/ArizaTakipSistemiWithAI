using MediatR;

namespace Application.Features.Commands.TenantCommands;

public class CreateTanantCommand : IRequest
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string CompanyName { get; set; }
    public string Password { get; set; }
    public string ConnectionString { get; set; }    
}