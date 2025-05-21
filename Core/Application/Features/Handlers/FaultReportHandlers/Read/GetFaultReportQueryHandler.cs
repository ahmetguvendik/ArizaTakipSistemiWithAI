using Application.Features.Queries.FaultReportQueries;
using Application.Features.Results.FaultReportResults;
using Application.Repositories;
using MediatR;

namespace Application.Features.Handlers.FaultReportHandlers.Read;

public class GetFaultReportQueryHandler  : IRequestHandler<GetFaultReportQuery, List<GetFaultReportResult>>
{
    private readonly IFaultReportRepository _faultReportRepository;

    public GetFaultReportQueryHandler(IFaultReportRepository faultReportRepository)
    {
         _faultReportRepository = faultReportRepository;
    }
    
    public async Task<List<GetFaultReportResult>> Handle(GetFaultReportQuery request, CancellationToken cancellationToken)
    {
        var values = await _faultReportRepository.GetAllAsync();
        return values.Select(x => new GetFaultReportResult      
        {
            Id = x.Id,
            Title = x.Title,
            Description = x.Description,
            ReporterName = x.ReporterName,
            ReporterPhone = x.ReporterPhone,
            ReporterEmail = x.ReporterEmail,
            CreatedAt = x.CreatedAt,
            Status = x.Status,
            AssignedByName = x.AssignedBy != null ? x.AssignedBy.NameSurname : "Atanmamış",
            AssignedToName = x.AssignedTo != null ? x.AssignedTo.NameSurname : "Atanmamış",
            MachineName = x.Machine != null ? x.Machine.Name : "Bilinmiyor"
            
        }).ToList();
    }
}