using Application.Features.Queries.StatisticsQueries;
using Application.Features.Results.StatisticsResults;
using Application.Repositories;
using MediatR;

namespace Application.Features.Handlers.StatisticsHandlers.Read;

public class GetAssignedlFaultCountQueryHandler : IRequestHandler<GetAssignedlFaultCountQuery, GetAssignedlFaultCountQueryResult>
{
    private readonly IStatisticsRepository _statisticsRepository;

    public GetAssignedlFaultCountQueryHandler(IStatisticsRepository statisticsRepository)
    {
         _statisticsRepository = statisticsRepository;
    }
    
    public async Task<GetAssignedlFaultCountQueryResult> Handle(GetAssignedlFaultCountQuery request, CancellationToken cancellationToken)
    {
        var values =  _statisticsRepository.GetAssignedFaultCount();
        return new GetAssignedlFaultCountQueryResult()
        {
            GetAssignedFaultCount = values
        };
    }
}
