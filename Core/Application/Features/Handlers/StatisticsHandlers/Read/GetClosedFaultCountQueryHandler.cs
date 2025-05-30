using Application.Features.Queries.StatisticsQueries;
using Application.Features.Results.StatisticsResults;
using Application.Repositories;
using MediatR;

namespace Application.Features.Handlers.StatisticsHandlers.Read;

public class GetClosedFaultCountQueryHandler : IRequestHandler<GetClosedFaultCountQuery, GetClosedFaultCountQueryResult>
{
    private readonly IStatisticsRepository _statisticsRepository;

    public GetClosedFaultCountQueryHandler(IStatisticsRepository statisticsRepository)
    {
        _statisticsRepository = statisticsRepository;
    }
    public async Task<GetClosedFaultCountQueryResult> Handle(GetClosedFaultCountQuery request, CancellationToken cancellationToken)
    {
        var value = _statisticsRepository.GetClosedFaultCount();
        return new GetClosedFaultCountQueryResult()
        {
            GetClosedFaultCount = value
        };
    }
}