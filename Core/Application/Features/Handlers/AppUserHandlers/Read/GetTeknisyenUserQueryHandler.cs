using Application.Features.Queries.AppUserQueries;
using Application.Features.Results.AppUserResults;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Handlers.AppUserHandlers.Read;

public class GetTeknisyenUserQueryHandler : IRequestHandler<GetTeknisyenUserQuery, List<GetTeknisyenUserQueryResult>>
{
    private readonly UserManager<AppUser> _userManager;

    public GetTeknisyenUserQueryHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<List<GetTeknisyenUserQueryResult>> Handle(GetTeknisyenUserQuery request, CancellationToken cancellationToken)
    {
        var teknisyenler = await _userManager.GetUsersInRoleAsync("Teknisyen");

        return teknisyenler.Select(user => new GetTeknisyenUserQueryResult
        {
            Id = user.Id,
            Name = user.NameSurname
        }).ToList();
    }
}