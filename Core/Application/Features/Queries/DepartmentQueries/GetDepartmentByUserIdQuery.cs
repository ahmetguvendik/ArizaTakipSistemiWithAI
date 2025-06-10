using Application.Features.Results.DepartmentResults;
using MediatR;

namespace Application.Features.Queries;

public class GetDepartmentByUserIdQuery : IRequest<List<GetDepartmentByUserIdQueryResult>>
{
    public string Id { get; set; }

    public GetDepartmentByUserIdQuery(string id)
    {
         Id = id;
    }
}