using Application.Features.Queries.MachineQueries;
using Application.Features.Results.MachineResults;
using Application.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.Handlers.MachineHandlers.Read;

public class GetMachineByDepartmanIdQueryHandler : IRequestHandler<GetMachineByDepartmanIdQuery,List<GetMachineByDepartmanIdQueryResult>>
{
    private readonly IMachineRepository _machineRepository;

    public GetMachineByDepartmanIdQueryHandler(IMachineRepository machineRepository)
    {
         _machineRepository = machineRepository;
    }
    
    public async Task<List<GetMachineByDepartmanIdQueryResult>> Handle(GetMachineByDepartmanIdQuery request, CancellationToken cancellationToken)
    {
        var values = await _machineRepository.GetMachineByDepartmanIdAsync(request.Id);
        return values.Select(x=> new  GetMachineByDepartmanIdQueryResult()
        {
            Id = x.Id,
            Name = x.Name,
        }).ToList();
    }
}