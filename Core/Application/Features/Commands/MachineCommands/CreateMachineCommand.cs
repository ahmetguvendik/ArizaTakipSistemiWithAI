using MediatR;

namespace Application.Features.Commands.MachineCommands;

public class CreateMachineCommand : IRequest
{
    public string Name { get; set; }
    public string SerialNumber { get; set; }
    public string DepartmentId { get; set; }
}