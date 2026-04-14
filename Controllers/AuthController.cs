using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using breakincycleapi.Database;
using breakincycleapi.DTO_s;

namespace breakincycleapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    // IConfiguration lets us read values from appsettings.json
    public AuthController(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    // POST: api/Auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        // 1. Find the user by email in the database
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null)
            return Unauthorized(new { message = "Invalid email or password." });

        // 2. Verify the password using BCrypt
        //    BCrypt.Verify compares the plain password the client sent
        //    against the hashed password stored in the database
        bool passwordMatches = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
        if (!passwordMatches)
            return Unauthorized(new { message = "Invalid email or password." });

        // 3. Build the JWT token
        var token = GenerateToken(user.Id.ToString(), user.Email, user.Name);

        return Ok(new { token });
    }

    private string GenerateToken(string userId, string email, string name)
    {
        // Read the secret key from appsettings.json
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Claims are pieces of info stored inside the token
        // The frontend can read these without calling the server
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),   // user's ID
            new Claim(JwtRegisteredClaimNames.Email, email),  // user's email
            new Claim("name", name),                          // user's name
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // unique token ID
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8), // token expires after 8 hours
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
