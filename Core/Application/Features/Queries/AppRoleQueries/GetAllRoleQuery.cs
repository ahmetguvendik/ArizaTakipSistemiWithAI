using Application.Features.Results.AppRoleResults;
using MediatR;

namespace Application.Features.Queries.AppRoleQueries;

public class GetAllRoleQuery : IRequest<List<GetAllRoleQueryResult>>
{
    
}