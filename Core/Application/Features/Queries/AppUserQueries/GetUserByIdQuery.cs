using Application.Features.Results.AppUserResults;
using MediatR;

namespace Application.Features.Queries.AppUserQueries;

public class GetUserByIdQuery : IRequest<GetUserByIdQueryResult>
{
    public string Id { get; set; }

    public GetUserByIdQuery(string id)
    {
         Id = id;
    }
}