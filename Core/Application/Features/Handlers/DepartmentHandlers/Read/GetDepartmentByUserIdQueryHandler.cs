using Application.Features.Queries;
using Application.Features.Results.DepartmentResults;
using Application.Repositories;
using MediatR;

namespace Application.Features.Handlers.DepartmentHandlers.Read;

public class GetDepartmentByUserIdQueryHandler : IRequestHandler<GetDepartmentByUserIdQuery, List<GetDepartmentByUserIdQueryResult>>
{
    private readonly IDepartmentRepository _departmentRepository;

    public GetDepartmentByUserIdQueryHandler(IDepartmentRepository departmentRepository)
    {
         _departmentRepository = departmentRepository;
    }
    
    public async Task<List<GetDepartmentByUserIdQueryResult>> Handle(GetDepartmentByUserIdQuery request, CancellationToken cancellationToken)
    {
        var values = await _departmentRepository.GetAllByUserIdAsync(request.Id);
        return values.Select(x => new GetDepartmentByUserIdQueryResult()
        {
            Id = x.Id,
            Name = x.Name,
        }).ToList();
    }
}