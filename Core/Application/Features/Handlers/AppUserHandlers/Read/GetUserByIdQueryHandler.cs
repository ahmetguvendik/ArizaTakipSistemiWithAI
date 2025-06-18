using Application.Features.Queries.AppUserQueries;
using Application.Features.Results.AppUserResults;
using Application.Repositories;
using MediatR;

namespace Application.Features.Handlers.AppUserHandlers.Read;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery,GetUserByIdQueryResult>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;   
    }
    public async Task<GetUserByIdQueryResult> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
       var user = await _userRepository.GetUserById(request.Id);
       return new GetUserByIdQueryResult()  
       {
           NameSurname = user.NameSurname,
           DepartmentName = user.Department.Name,
           Email = user.Email,
           Username = user.UserName,
       };
    }
}