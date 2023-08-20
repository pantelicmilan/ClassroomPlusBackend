using ClassroomPlus.DTOs;
using ClassroomPlus.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClassroomPlus.Repositories.Interfaces;

public interface IUserRepository
{
    public Task<IEnumerable<User>> getAllAsync();
    public Task<User> getUserByIdAsync(int id);
    public Task<User> createUserAsync(User user);
    public Task<User> editUserAsync(User user);
    public User deleteUser(User user);
    public Task<User> getUserByUsernameAsync(string username);
    public Task<User> getUserByEmailAsync(string email);
}
