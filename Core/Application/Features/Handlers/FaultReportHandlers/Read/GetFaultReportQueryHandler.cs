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
            AssignedTime = x.AssignedTime,
            ClosedTime = x.ClosedTime,  
            Status = x.Status,
            AssignedByName = x.AssignedBy != null
                ? x.AssignedBy.NameSurname
                : "Atanmamış",
            ClosedDescription = x.ClosedDescription,    
            AssignedToName = x.AssignedTo != null
                ? x.AssignedTo.NameSurname
                : null,
            MachineName = x.Machine != null
                ? x.Machine.Name    
                : "Bilinmiyor",
            AssignedToId = x.AssignedTo?.Id ?? null,
            DepartmanName = x.AssignedTo?.Department?.Name ?? null,
            DepartmanId = x.AssignedTo?.Department?.Id ?? null,
            MachineId = x.Machine?.Id ?? null,
            ClosedByName = x.ClosedBy?.NameSurname?? null,
            ClosedById = x.ClosedBy?.Id ?? null,
            AssignedById = x.AssignedBy?.Id ?? null,
        }).ToList();
    }
}