using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using breakincycleapi.Database;
using breakincycleapi.Database.Models;
using breakincycleapi.DTO_s;

namespace breakincycleapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // -------------------------------------------------------------
        // GET: api/Users/{id}
        // -------------------------------------------------------------
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound(new { message = "User not found." });

            return Ok(user);
        }

        // -------------------------------------------------------------
        // POST: api/Users
        // -------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDto dto)
        {
            if (dto == null)
                return BadRequest(new { message = "Invalid user data." });

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = dto.Password, // hash before storing in production
                Phonenumbar = dto.PhoneNumber,
                Location = dto.Location,
                Createdat = DateTime.UtcNow,
                Lastactive = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        // -------------------------------------------------------------
        // PUT: api/Users/{id}
        // -------------------------------------------------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] User updatedUser)
        {
            // Safety check to prevent mistakenly editing the wrong user
            if (id != updatedUser.Id)
                return BadRequest(new { message = "ID mismatch." });

            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
                return NotFound(new { message = "User not found." });

            // Safely update specific properties to avoid accidentally overwriting IDs or creation dates
            existingUser.Name = updatedUser.Name;
            existingUser.Email = updatedUser.Email;
            existingUser.PasswordHash = updatedUser.PasswordHash;
            existingUser.Phonenumbar = updatedUser.Phonenumbar;
            existingUser.Location = updatedUser.Location;
            existingUser.Lastactive = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(existingUser);
        }

        // -------------------------------------------------------------
        // DELETE: api/Users/{id}
        // -------------------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound(new { message = "User not found." });

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User deleted successfully." });
        }
    }
}
