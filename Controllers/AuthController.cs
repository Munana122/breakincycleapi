using breakincycleapi.Database;
using Microsoft.AspNetCore.Http.HttpResults;
using breakincycleapi.Database.Models;
using breakincycleapi.DTO_s;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace breakincycleapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthController(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    // POST: api/Auth/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserCreateDto dto)
    {
        bool emailExists = await _context.Users.AnyAsync(u => u.Email == dto.Email);
        if (emailExists)
            return BadRequest(new { message = "Email is already registered." });

        if (dto.Role != "Teacher" && dto.Role != "Student")
            return BadRequest(new { message = "Role must be 'Teacher' or 'Student'." });

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Phonenumbar = dto.PhoneNumber,
            Location = dto.Location,
            Role = dto.Role,
            Createdat = DateTime.UtcNow,
            Lastactive = DateTime.UtcNow
        };

        _context.Users.Add(user);

        if (dto.Role == "Teacher")
        {
            _context.Teachers.Add(new Teacher
            {
                TeacherId = user.Id,
                Name = dto.Name,
                Email = dto.Email,
                Phonenumber = dto.PhoneNumber,
                Location = dto.Location,
                Createdat = DateTime.UtcNow,
                Lastactive = DateTime.UtcNow
            });
        }
        else
        {
            _context.Students.Add(new Student
            {
                StudentId = user.Id,
                Name = dto.Name,
                Email = dto.Email,
                Phonenumber = dto.PhoneNumber,
                Location = dto.Location,
                Createdat = DateTime.UtcNow,
                Lastactive = DateTime.UtcNow
            });
        }

        await _context.SaveChangesAsync();

        var token = GenerateToken(user.Id.ToString(), user.Email, user.Name, user.Role);
        return Ok(new { token });
    }

    // POST: api/Auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null)
            return Unauthorized(new { message = "Invalid email or password." });

        bool passwordMatches = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
        if (!passwordMatches)
            return Unauthorized(new { message = "Invalid email or password." });

        var token = GenerateToken(user.Id.ToString(), user.Email, user.Name, user.Role);
        return Ok(new { token });
    }

    private string GenerateToken(string userId, string email, string name, string role)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim("name", name),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
