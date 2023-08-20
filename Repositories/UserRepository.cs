using ClassroomPlus.Data;
using ClassroomPlus.DTOs;
using ClassroomPlus.Entities;
using ClassroomPlus.Repositories.Interfaces;
using ClassroomPlus.Services;
using Microsoft.EntityFrameworkCore;

namespace ClassroomPlus.Repositories;

public class UserRepository : IUserRepository
{
    private readonly SQLServerContext _context;
    public UserRepository(SQLServerContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> getAllAsync() 
    {
        var users = await _context.Users.Include(u=> u.Classrooms).ToListAsync();
        return users;
    }

    public async Task<User> getUserByIdAsync(int id) 
    {
        var user = await _context.Users
            .Include(u => u.Classrooms)
            .FirstOrDefaultAsync(currUser => currUser.Id == id);

        return user;
    }

    public async Task<User> createUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        return user;
    }

    public async Task<User> editUserAsync(User user)
    {
        var currentUser = await _context.Users.FirstOrDefaultAsync
            (currentUser => currentUser.Id == user.Id);

            currentUser.Username = user.Username;


            currentUser.Surname = user.Surname;

            currentUser.Name = user.Name;

            if(user.ProfileImageUrl != null)
            {
                currentUser.ProfileImageUrl = user.ProfileImageUrl;
            }


        return currentUser;
    }

    public User deleteUser(User user)
    {
        _context.Users.Remove(user);
        return user;
    }

    public async Task<User> getUserByUsernameAsync(string username)
    {
        var user = await _context.Users.Include(u=> u.Classrooms)
            .FirstOrDefaultAsync(u => u.Username == username);
        return user;
    }

    public async Task<User> getUserByEmailAsync(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        return user;
    }
}
