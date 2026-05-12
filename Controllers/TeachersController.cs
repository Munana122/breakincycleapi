using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using breakincycleapi.Database;
using breakincycleapi.Database.Models;
using breakincycleapi.DTO_s;
using breakincycleapi.Services;

namespace breakincycleapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeachersController : ControllerBase
{
    private readonly ITeacherService _teacherService;

    public TeachersController(ITeacherService teacherService)
    {
        _teacherService = teacherService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTeachers()
    {
        var teachers = await _teacherService.GetAllTeachersAsync();
        return Ok(teachers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTeacherById(Guid id)
    {
        var teacher = await _teacherService.GetTeacherByIdAsync(id);
        if (teacher == null) return NotFound(new { message = "Teacher not found." });

        return Ok(teacher);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTeacher([FromBody] TeacherCreateDto dto)
    {
        var newId = await _teacherService.CreateTeacherAsync(dto);
        return CreatedAtAction(nameof(GetTeacherById), new { id = newId }, null);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTeacherAsync(Guid id, TeacherUpdateDTO dto)
    {
        var success = await _teacherService.UpdateTeacherAsync(id, dto);
        if (!success)
        {
            return NotFound(new { message = "Teacher not found." });
        }
        return Ok(success);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTeacher(Guid id)
    {
        var teacher = await _teacherService.DeleteTeacherAsync(id);
        if (teacher == null) return NotFound(new { message = "Teacher not found." });
        return Ok(new { message = "Teacher deleted successfully." });
    }
}