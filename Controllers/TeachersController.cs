using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using breakincycleapi.Database;
using breakincycleapi.Database.Models;

namespace breakincycleapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeachersController : ControllerBase
{
    private readonly AppDbContext _context;

    public TeachersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTeachers()
    {
        var teachers = await _context.Teachers.ToListAsync();
        return Ok(teachers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTeacherById(Guid id)
    {
        var teacher = await _context.Teachers.FindAsync(id);
        if (teacher == null) return NotFound(new { message = "Teacher not found." });

        return Ok(teacher);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTeacher([FromBody] Teacher teacher)
    {
        if (teacher == null) return BadRequest(new { message = "Invalid teacher data." });

        teacher.TeacherId = Guid.NewGuid();
        teacher.Createdat = DateTime.UtcNow;
        teacher.Lastactive = DateTime.UtcNow;

        _context.Teachers.Add(teacher);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTeacherById), new { id = teacher.TeacherId }, teacher);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTeacher(Guid id, [FromBody] Teacher updatedTeacher)
    {
        if (id != updatedTeacher.TeacherId) return BadRequest(new { message = "ID mismatch." });

        var existingTeacher = await _context.Teachers.FindAsync(id);
        if (existingTeacher == null) return NotFound(new { message = "Teacher not found." });

        existingTeacher.Name = updatedTeacher.Name;
        existingTeacher.Email = updatedTeacher.Email;
        existingTeacher.Phonenumber = updatedTeacher.Phonenumber; // Fixed typo "Phonenumbar" from previous models based on new class properties
        existingTeacher.Location = updatedTeacher.Location;
        existingTeacher.Lastactive = DateTime.UtcNow;

        _context.Teachers.Update(existingTeacher);
        await _context.SaveChangesAsync();

        return Ok(existingTeacher);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTeacher(Guid id)
    {
        var teacher = await _context.Teachers.FindAsync(id);
        if (teacher == null) return NotFound(new { message = "Teacher not found." });

        _context.Teachers.Remove(teacher);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Teacher deleted successfully." });
    }
}