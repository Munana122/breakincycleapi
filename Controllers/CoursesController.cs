using breakincycleapi.Database;
using breakincycleapi.Database.Models;
using breakincycleapi.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace breakincycleapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly AppDbContext _context;

    public CoursesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCourses()
    {
        var courses = await _context.Courses.ToListAsync();
        return Ok(courses);
    }

    [HttpGet("controller/{id}", Name = "GetCourseByIdController")]
    public async Task<IActionResult> GetCourseById(Guid id)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course == null)
            return NotFound(new { message = "Course not found." });

        return Ok(course);
    }

    [HttpPost]
    public async Task<ActionResult<Course>> PostCourse(CourseCreateDto courseDto)
    {
        // Map the DTO to the real Database Model
        var course = new Course
        {
            CourseId = Guid.NewGuid(), // Manually set if your DB doesn't
            Name = courseDto.Name,
            Description = courseDto.Description,
            Createdat = DateTime.UtcNow,
            Lastactive = DateTime.UtcNow
        };

        _context.Courses.Add(course);
        await _context.SaveChangesAsync();

        return CreatedAtRoute("GetCourseByIdController", new { id = course.CourseId }, course);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCourse(Guid id, [FromBody] Course updatedCourse)
    {
        if (id != updatedCourse.CourseId)
            return BadRequest(new { message = "ID mismatch." });

        var existingCourse = await _context.Courses.FindAsync(id);
        if (existingCourse == null)
            return NotFound(new { message = "Course not found." });

        existingCourse.Name = updatedCourse.Name;
        existingCourse.Description = updatedCourse.Description;
        existingCourse.Lastactive = DateTime.UtcNow;

        _context.Courses.Update(existingCourse);
        await _context.SaveChangesAsync();

        return Ok(existingCourse);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCourse(Guid id)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course == null)
            return NotFound(new { message = "Course not found." });

        _context.Courses.Remove(course);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Course deleted successfully." });
    }
}
