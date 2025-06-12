using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Repositories;

public class UserRepository  : IUserRepository
{
    private readonly FaultDbContext _context;

    public UserRepository(FaultDbContext context)
    {
         _context = context;
    }
    
    public async Task<AppUser> GetUserById(string id)
    {
        var user = await _context.Users.Include(x=>x.Department).FirstOrDefaultAsync(x => x.Id == id);
        return user;
    }
}