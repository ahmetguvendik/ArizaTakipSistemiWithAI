using Application.Repositories;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Repositories;

public class UserRepository  : IUserRepository
{
    private readonly FaultDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public UserRepository(FaultDbContext context, UserManager<AppUser> userManager)
    {
         _context = context;
         _userManager = userManager;
    }
    
    public async Task<AppUser> GetUserById(string id)
    {
        var user = await _context.Users.Include(x=>x.Department).FirstOrDefaultAsync(x => x.Id == id);
        return user;
        
    }

   
}