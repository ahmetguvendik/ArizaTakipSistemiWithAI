using System.Security.Principal;
using MediatR;

namespace Application.Features.Commands.FaultReportComamnds;

public class AssignTechnicianCommand : IRequest
{
    public string Id { get; set; }
    public string AssignnedToId { get; set; }
    public string Statues { get; set; } 
}