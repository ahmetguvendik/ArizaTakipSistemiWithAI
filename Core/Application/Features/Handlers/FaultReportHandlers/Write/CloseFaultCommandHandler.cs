using Application.Features.Commands.FaultReportComamnds;
using Application.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.Handlers.FaultReportHandlers.Write;

public class CloseFaultCommandHandler : IRequestHandler<CloseFaultCommand>
{
    private readonly IRepository<FaultReport> _faultReportRepository;

    public CloseFaultCommandHandler(IRepository<FaultReport> faultReportRepository)
    {
        _faultReportRepository = faultReportRepository;
    }
    public async Task Handle(CloseFaultCommand request, CancellationToken cancellationToken)
    {
        var value = await _faultReportRepository.GetByIdAsync(request.Id);
        value.ClosedById = request.ClosedById;
        value.MachineId = request.MachineId;
        value.ClosedTime = DateTime.Now;
        value.ClosedDescription = request.FaultDescription;
        value.Status = "Kapandı";
        await _faultReportRepository.UpdateAsync(value);
        await _faultReportRepository.SaveChangesAsync();
        
    }
}

