using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using breakincycleapi.Database;
using breakincycleapi.Database.Models;
using breakincycleapi.DTO_s;

namespace breakincycleapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeacherCoursesController : ControllerBase
{
    private readonly AppDbContext _context;

    public TeacherCoursesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTeacherCourses()
    {
        var teacherCourses = await _context.TeacherCourses.ToListAsync();
        return Ok(teacherCourses);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTeacherCourseById(long id) // TeacherCourseId is 'long'
    {
        var teacherCourse = await _context.TeacherCourses.FindAsync(id);
        if (teacherCourse == null) return NotFound(new { message = "TeacherCourse mapping not found." });

        return Ok(teacherCourse);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTeacherCourse([FromBody] TeacherCourseCreateDto dto)
    {
        if (dto == null) return BadRequest(new { message = "Invalid data." });

        var teacherCourse = new TeacherCourse
        {
            // TeacherCourseId (long) is usually auto-incremented by SQL Server
            TeacherId = dto.TeacherId,
            CourseId = dto.CourseId,
            AssignedAt = DateTime.UtcNow
        };

        _context.TeacherCourses.Add(teacherCourse);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTeacherCourseById), new { id = teacherCourse.TeacherCourseId }, teacherCourse);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTeacherCourse(long id)
    {
        var teacherCourse = await _context.TeacherCourses.FindAsync(id);
        if (teacherCourse == null) return NotFound(new { message = "TeacherCourse mapping not found." });

        _context.TeacherCourses.Remove(teacherCourse);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Mapping deleted successfully." });
    }
}