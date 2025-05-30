using Application.Features.Queries.StatisticsQueries;
using Application.Features.Results.StatisticsResults;
using Application.Repositories;
using MediatR;

namespace Application.Features.Handlers.StatisticsHandlers.Read;

public class GetTotalFaultCountQueryHandler : IRequestHandler<GetTotalFaultCountQuery, GetTotalFaultCountQueryResult>
{
    private readonly IStatisticsRepository _statisticsRepository;

    public GetTotalFaultCountQueryHandler(IStatisticsRepository statisticsRepository)
    {
        _statisticsRepository = statisticsRepository;
    }

    public async Task<GetTotalFaultCountQueryResult> Handle(GetTotalFaultCountQuery request, CancellationToken cancellationToken)
    {
        var count = _statisticsRepository.GetTotalFaultCount();
        return new GetTotalFaultCountQueryResult()
        {
            GetTotalFaultCount = count
        };
    }
}