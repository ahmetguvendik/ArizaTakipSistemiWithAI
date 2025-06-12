using Domain.Entities;

namespace Application.Repositories;

public interface IUserRepository
{
    Task<AppUser> GetUserById(string id);
}