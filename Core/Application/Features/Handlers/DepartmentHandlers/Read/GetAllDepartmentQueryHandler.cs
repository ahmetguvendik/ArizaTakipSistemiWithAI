using Application.Features.Queries;
using Application.Features.Results.DepartmentResults;
using Application.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.Handlers.DepartmentHandlers.Read;

public class GetAllDepartmentQueryHandler : IRequestHandler<GetAllDepartmentQuery, List<GetAllDepartmentQueryResult>>
{
    private readonly IRepository<Department> _repository;

    public GetAllDepartmentQueryHandler(IRepository<Department> repository)
    {
         _repository = repository;
    }
    
    public async Task<List<GetAllDepartmentQueryResult>> Handle(GetAllDepartmentQuery request, CancellationToken cancellationToken)
    {
        var list = await _repository.GetAllAsync();
        return list.Select(x => new GetAllDepartmentQueryResult()
        {
            Id = x.Id,
            Name = x.Name,
        }).ToList();
    }
}