using Application.Features.Commands.FaultReportComamnds;
using Application.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.Handlers.FaultReportHandlers.Write;

public class CreateFaultReportCommandHandler : IRequestHandler<CreateFaultReportCommand>
{
    private readonly IRepository<FaultReport>  _faultReportRepository;

    public CreateFaultReportCommandHandler(IRepository<FaultReport> faultReportRepository)
    {
         _faultReportRepository = faultReportRepository;
    }
    
    public async Task Handle(CreateFaultReportCommand request, CancellationToken cancellationToken)
    {
       var  faultReport = new FaultReport();
       faultReport.Id = Guid.NewGuid().ToString();
       faultReport.Title = request.Title;
       faultReport.Description = request.Description;
       faultReport.Status = "Yeni";
       faultReport.CreatedAt = DateTime.UtcNow;
       faultReport.ReporterName  = request.ReporterName;
       faultReport.ReporterEmail  = request.ReporterEmail;
       faultReport.ReporterPhone  = request.ReporterPhone;
       await _faultReportRepository.CreateAsync(faultReport);
       await _faultReportRepository.SaveChangesAsync();
       
    }
}