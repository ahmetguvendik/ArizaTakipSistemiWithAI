using Application.Features.Results.FaultReportResults;
using MediatR;

namespace Application.Features.Queries.FaultReportQueries;

public class GetFaultReportByIdQuery : IRequest<GetFaultReportByIdQueryResult>
{
    public string Id { get; set; }

    public GetFaultReportByIdQuery(string id)
    {
         Id = id;
    }
}