using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using breakincycleapi.Database;
using breakincycleapi.Database.Models;
using MediatR;

namespace breakincycleapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public StudentsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllStudents()
    {
        var students = await _context.Students.ToListAsync();
        return Ok(students);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStudentById(Guid id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null)
            return NotFound(new { message = "Student not found." });

        return Ok(student);
    }

    [HttpPost]
    public async Task<IActionResult> CreateStudent([FromBody] Student student)
    {
        if (student == null)
            return BadRequest(new { message = "Invalid student data." });

        student.StudentId = Guid.NewGuid();
        student.Createdat = DateTime.UtcNow;
        student.Lastactive = DateTime.UtcNow;

        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetStudentById), new { id = student.StudentId }, student);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudent(Guid id, [FromBody] Student updatedStudent)
    {
        if (id != updatedStudent.StudentId)
            return BadRequest(new { message = "ID mismatch." });

        var existingStudent = await _context.Students.FindAsync(id);
        if (existingStudent == null)
            return NotFound(new { message = "Student not found." });

        existingStudent.Name = updatedStudent.Name;
        existingStudent.Email = updatedStudent.Email;
        existingStudent.Phonenumbar = updatedStudent.Phonenumbar;
        existingStudent.Location = updatedStudent.Location;
        existingStudent.Lastactive = DateTime.UtcNow;

        _context.Students.Update(existingStudent);
        await _context.SaveChangesAsync();

        return Ok(existingStudent);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(Guid id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null)
            return NotFound(new { message = "Student not found." });

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Student deleted successfully." });
    }
}
