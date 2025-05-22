using Application.Features.Results.MachineResults;
using MediatR;

namespace Application.Features.Queries.MachineQueries;

public class GetMachineByDepartmanIdQuery : IRequest<List<GetMachineByDepartmanIdQueryResult>>
{
    public string Id { get; set; }

    public GetMachineByDepartmanIdQuery(string id)
    { 
       Id = id;  
    }
}