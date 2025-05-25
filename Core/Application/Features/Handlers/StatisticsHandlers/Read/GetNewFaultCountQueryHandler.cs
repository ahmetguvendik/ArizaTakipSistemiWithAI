using Application.Features.Queries.StatisticsQueries;
using Application.Features.Results.StatisticsResults;
using Application.Repositories;
using MediatR;

namespace Application.Features.Handlers.StatisticsHandlers.Read;

public class GetNewFaultCountQueryHandler : IRequestHandler<GetNewFaultCountQuery, GetNewFaultCountQueryResult>
{
    private readonly IStatisticsRepository _repository;

    public GetNewFaultCountQueryHandler(IStatisticsRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetNewFaultCountQueryResult> Handle(GetNewFaultCountQuery request, CancellationToken cancellationToken)
    {
        var value = _repository.GetNewFaultCount();
        return new GetNewFaultCountQueryResult()
        {
            GetNewFaultCount = value
        };
    }
}