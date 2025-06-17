using Application.Features.Queries.AppRoleQueries;
using Application.Features.Results.AppRoleResults;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Handlers.AppRoleHandlers;

public class GetAllRoleQueryHandlers  : IRequestHandler<GetAllRoleQuery, List<GetAllRoleQueryResult>>
{
    private readonly RoleManager<AppRole>  _roleManager;

    public GetAllRoleQueryHandlers(RoleManager<AppRole> roleManager)
    {
       _roleManager =  roleManager;
    }
    public async Task<List<GetAllRoleQueryResult>> Handle(GetAllRoleQuery request, CancellationToken cancellationToken)
    {
       var roles = await _roleManager.Roles.ToListAsync();
       return roles.Select(x => new GetAllRoleQueryResult()
       {
           Id = x.Id,
           Name = x.Name,
       }).ToList();
    }
}