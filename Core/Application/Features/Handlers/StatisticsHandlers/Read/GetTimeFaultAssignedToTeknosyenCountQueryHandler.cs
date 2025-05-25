using Application.Features.Queries.StatisticsQueries;
using Application.Features.Results.StatisticsResults;
using Application.Repositories;
using MediatR;

namespace Application.Features.Handlers.StatisticsHandlers.Read;

public class GetTimeFaultAssignedToTeknosyenCountQueryHandler : IRequestHandler<GetTimeFaultAssignedToTeknosyenCountQuery,GetTimeFaultAssignedToTeknosyenCountQueryResult>
{
    private readonly IStatisticsRepository _repository;

    public GetTimeFaultAssignedToTeknosyenCountQueryHandler(IStatisticsRepository repository)
    {
         _repository = repository;
    }
    public async Task<GetTimeFaultAssignedToTeknosyenCountQueryResult> Handle(GetTimeFaultAssignedToTeknosyenCountQuery request, CancellationToken cancellationToken)
    {
        var value = _repository.GetAverageAssignmentTimeInMinutes();
        return new GetTimeFaultAssignedToTeknosyenCountQueryResult()
        {
            GetTimeFaultAssignedToTeknosyenCountTime = value
        };
    }
}