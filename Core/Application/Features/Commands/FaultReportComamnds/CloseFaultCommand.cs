using MediatR;

namespace Application.Features.Commands.FaultReportComamnds;

public class CloseFaultCommand : IRequest   
{
    public string Id { get; set; }
    public string MachineId { get; set; } 
    public string FaultDescription { get; set; }
    public string ClosedById { get; set; }
    public string Status { get; set; }  
    public DateTime ClosedTime { get; set; }        
}
