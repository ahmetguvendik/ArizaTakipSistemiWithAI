using Application.Features.Results.DepartmentResults;
using MediatR;

namespace Application.Features.Queries;

public class GetAllDepartmentQuery : IRequest<List<GetAllDepartmentQueryResult>>
{
    
}