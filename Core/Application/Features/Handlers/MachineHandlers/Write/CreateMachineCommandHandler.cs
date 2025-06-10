using System.Reflection.PortableExecutable;
using Application.Features.Commands.MachineCommands;
using Application.Repositories;
using MediatR;

namespace Application.Features.Handlers.MachineHandlers.Write;

public class CreateMachineCommandHandler : IRequestHandler<CreateMachineCommand>
{
    private readonly IRepository<Domain.Entities.Machine> _repository;

    public CreateMachineCommandHandler(IRepository<Domain.Entities.Machine> repository)
    {
         _repository  = repository;
    }
    
    public async Task Handle(CreateMachineCommand request, CancellationToken cancellationToken)
    {
        var machine = new Domain.Entities.Machine();
        machine.Id = Guid.NewGuid().ToString();
        machine.Name = request.Name;
        machine.SerialNumber = request.SerialNumber;
        machine.DepartmentId = request.DepartmentId;
        await _repository.CreateAsync(machine);
        await  _repository.SaveChangesAsync();
    }
}