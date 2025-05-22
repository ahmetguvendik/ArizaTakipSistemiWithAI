using Application.Features.Commands.FaultReportComamnds;
using Application.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.Handlers.FaultReportHandlers.Write;

public class AssignTechnicianCommandHandler : IRequestHandler<AssignTechnicianCommand>
{
    private readonly IRepository<FaultReport>  _faultReportRepository;

    public AssignTechnicianCommandHandler(IRepository<FaultReport> faultReportRepository)
    {
        _faultReportRepository = faultReportRepository;
    }
    public async Task Handle(AssignTechnicianCommand request, CancellationToken cancellationToken)
    {
        var value = await _faultReportRepository.GetByIdAsync(request.Id);
        value.AssignedToId = request.AssignnedToId;
        value.Id = request.Id;
        value.Status = "Atandı";
        value.AssignedById = request.AssignnedById;
        value.AssignedTime = request.AssignedTime;  
        await _faultReportRepository.UpdateAsync(value);
        await _faultReportRepository.SaveChangesAsync();
    }
}