using Application.Features.Results.FaultReportResults;
using MediatR;

namespace Application.Features.Queries.FaultReportQueries;

public class GetFaultReportByDepartmanIdQuery : IRequest<List<GetFaultReportByDepartmanIdQueryResult>>
{
    public string Id { get; set; }

    public GetFaultReportByDepartmanIdQuery(string id)
    {
         Id = id;
    }
}