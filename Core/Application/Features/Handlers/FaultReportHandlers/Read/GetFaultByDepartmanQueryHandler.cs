using Application.Features.Queries.FaultReportQueries;
using Application.Features.Results.FaultReportResults;
using Application.Repositories;
using MediatR;

namespace Application.Features.Handlers.FaultReportHandlers.Read;

public class GetFaultByDepartmanQueryHandler : IRequestHandler<GetFaultByDepartmanQuery, List<GetFaultByDepartmanQueryResult>>
{
    private readonly IFaultReportRepository _faultReportRepository;

    public GetFaultByDepartmanQueryHandler(IFaultReportRepository faultReportRepository)
    {
         _faultReportRepository = faultReportRepository;
    }
    
    public async Task<List<GetFaultByDepartmanQueryResult>> Handle(GetFaultByDepartmanQuery request, CancellationToken cancellationToken)
    {
        var values = await _faultReportRepository.GetFaultByDepartmanAsync();
        return values.ToList();
    }
}