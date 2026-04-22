using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using breakincycleapi.Database;
using breakincycleapi.Database.Models;
using breakincycleapi.DTO_s;

namespace breakincycleapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProgressController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProgressController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProgresses()
    {
        var progresses = await _context.Progresses
            .Include(p => p.Student)   // load the full Student object
            .Include(p => p.Course)    // load the full Course object
            .Select(p => new
            {
                p.ProgressId,
                p.StudentId,
                p.CourseId,
                p.ProgressStatus,
                p.LastUpdated,
                course = p.Course.Name,
                student = p.Student.Name
            })
            .ToListAsync();
        return Ok(progresses);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProgressById(long id)
    {
        var progress = await _context.Progresses
            .Include(p => p.Student)
            .Include(p => p.Course)
            .Where(p => p.ProgressId == id)
            .Select(p => new
            {
                p.ProgressId,
                p.StudentId,
                p.CourseId,
                p.ProgressStatus,
                p.LastUpdated,
                course = p.Course.Name,
                student = p.Student.Name
            })
            .FirstOrDefaultAsync();

        if (progress == null) return NotFound(new { message = "Progress record not found." });
        return Ok(progress);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProgress([FromBody] ProgressCreateDto dto)
    {
        if (dto == null) return BadRequest(new { message = "Invalid data." });

        var progress = new Progress
        {
            StudentId = dto.StudentId,
            CourseId = dto.CourseId,
            ProgressStatus = dto.ProgressStatus,
            LastUpdated = DateTime.UtcNow
        };

        _context.Progresses.Add(progress);
        int v = await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProgressById), new { id = progress.ProgressId }, progress);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProgress(long id, [FromBody] ProgressCreateDto dto)
    {
        var existingProgress = await _context.Progresses.FindAsync(id);
        if (existingProgress == null) return NotFound(new { message = "Progress record not found." });

        existingProgress.ProgressStatus = dto.ProgressStatus;
        existingProgress.LastUpdated = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(existingProgress);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProgress(long id)
    {
        var progress = await _context.Progresses.FindAsync(id);
        if (progress == null) return NotFound(new { message = "Progress record not found." });

        _context.Progresses.Remove(progress);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Progress deleted successfully." });
    }
}
