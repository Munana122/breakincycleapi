using breakincycleapi.DTO_s;
using breakincycleapi.Services;
using Microsoft.AspNetCore.Mvc;

namespace breakincycleapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICourceService _courseService;

    public CoursesController(ICourceService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCourses()
    {
        var courses = await _courseService.GetAllCoursesAsync();
        return Ok(courses);
    }

    [HttpGet("{id}", Name = "GetCourseByIdController")]
    public async Task<IActionResult> GetCourseById(Guid id)
    {
        var course = await _courseService.GetCourseByIdAsync(id);
        if (course == null)
            return NotFound(new { message = "Course not found." });
        return Ok(course);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCourse(CourseCreateDto courseDto)
    {
        var courseId = await _courseService.CreateCourseAsync(courseDto);
        return CreatedAtRoute("GetCourseByIdController", new { id = courseId }, new { id = courseId });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCourse(Guid id, [FromBody] CourseUpdateDTO dto)
    {
        if (id != dto.CourseId)
            return BadRequest(new { message = "ID mismatch." });
        var updatedId = await _courseService.UpdateCourseAsync(dto);
        if (updatedId == Guid.Empty)
            return NotFound(new { message = "Course not found." });
        return Ok(new { id = updatedId });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCourse(Guid id)
    {
        var deleted = await _courseService.DeleteCourseAsync(id);
        if (!deleted)
            return NotFound(new { message = "Course not found." });
        return Ok(new { message = "Course deleted successfully." });
    }
}
