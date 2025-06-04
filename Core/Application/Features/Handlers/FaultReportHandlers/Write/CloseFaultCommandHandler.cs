using Application.Features.Commands.FaultReportComamnds;
using Application.Repositories;
using Application.Services;
using Domain.Entities;
using MediatR;

namespace Application.Features.Handlers.FaultReportHandlers.Write;

public class CloseFaultCommandHandler : IRequestHandler<CloseFaultCommand>
{
    private readonly IRepository<FaultReport> _faultReportRepository;
    private readonly IEmailService  _emailService;
    
    public CloseFaultCommandHandler(IRepository<FaultReport> faultReportRepository, IEmailService emailService)
    {
        _faultReportRepository = faultReportRepository;
        _emailService = emailService;
    }
    public async Task Handle(CloseFaultCommand request, CancellationToken cancellationToken)
    {
        var value = await _faultReportRepository.GetByIdAsync(request.Id);
        value.ClosedById = request.ClosedById;
        value.MachineId = request.MachineId;
        value.ClosedTime = DateTime.Now;
        value.ClosedDescription = request.FaultDescription;
        value.Status = "Kapandı";
        await _emailService.SendClosedFaultEmailAsync(value.ReporterEmail,$"Acmis Oldugunuz Arizaniniz : {value.Description} Basarili Bir Sekilde Kapatilmistir");
        await _faultReportRepository.UpdateAsync(value);
        await _faultReportRepository.SaveChangesAsync();
        
    }
}

