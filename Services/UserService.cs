using breakincycleapi.Database;
using breakincycleapi.Database.Models;
using breakincycleapi.DTO_s;
using breakincycleapi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync<User>();
    }

    public async Task<User?> GetUsersByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<bool> UpdateUserAsync(Guid id, UserUpdateDto userDto)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return false;

        user.Name = userDto.Name;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteUserAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Guid> CreateUserAsync(UserCreateDto userDto)
    {
        var newUser = new User
        {
            Id = Guid.NewGuid(),
            Name = userDto.Name,
            Email = userDto.Email,
            Role = userDto.Role,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
            Phonenumbar = userDto.PhoneNumber,
            Location = userDto.Location,
            Createdat = DateTime.UtcNow,
            Lastactive = DateTime.UtcNow
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();
        return newUser.Id;
    }
}