using Application.Features.Queries.StatisticsQueries;
using Application.Features.Results.StatisticsResults;
using Application.Repositories;
using MediatR;

namespace Application.Features.Handlers.StatisticsHandlers.Read;
 
public class GetAverageClosedTimeInMinutesQueryHandler : IRequestHandler<GetAverageClosedTimeInMinutesQuery,GetAverageClosedTimeInMinutesQueryResult>
{
    private readonly IStatisticsRepository  _repository;

    public GetAverageClosedTimeInMinutesQueryHandler(IStatisticsRepository repository)
    {
         _repository = repository;
    }
    
    public async Task<GetAverageClosedTimeInMinutesQueryResult> Handle(GetAverageClosedTimeInMinutesQuery request, CancellationToken cancellationToken)
    {
       var value = _repository.GetAverageClosedTimeInMinutes();
       return new GetAverageClosedTimeInMinutesQueryResult()
       {
           GetAverageClosedTimeInMinutes = value
       };   
    }
}