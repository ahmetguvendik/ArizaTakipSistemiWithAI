using Application.Features.Queries.FaultReportQueries;
using Application.Features.Results.FaultReportResults;
using Application.Repositories;
using MediatR;

namespace Application.Features.Handlers.FaultReportHandlers.Read;

public class GetFaultReportByIdQueryHandler : IRequestHandler<GetFaultReportByIdQuery, GetFaultReportByIdQueryResult>
{ 
    private readonly IFaultReportRepository _faultReportRepository;

    public GetFaultReportByIdQueryHandler(IFaultReportRepository faultReportRepository)
    {
         _faultReportRepository = faultReportRepository;
    }
    public async Task<GetFaultReportByIdQueryResult> Handle(GetFaultReportByIdQuery request, CancellationToken cancellationToken)
    {
        var values = await _faultReportRepository.GetFaultByIdAsync(request.Id);
        return new GetFaultReportByIdQueryResult
        {
            Id = values.Id,
            Title = values.Title,
            Description = values.Description,
            ReporterName = values.ReporterName,
            ReporterPhone = values.ReporterPhone,
            ReporterEmail = values.ReporterEmail,
            CreatedAt = values.CreatedAt,
            Status = values.Status,
            AssignedByName = values.AssignedBy != null ? values.AssignedBy.NameSurname : "Atanmamış",
            AssignedToName = values.AssignedTo != null ? values.AssignedTo.NameSurname : "Atanmamış",
            MachineName = values.Machine != null ? values.Machine.Name : "Bilinmiyor",
            DepartmanName = values.AssignedTo?.Department?.Name ?? "Departman atanmadı"
  
        };
        
    }
}