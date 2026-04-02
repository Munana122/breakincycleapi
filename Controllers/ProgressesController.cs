using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using breakincycleapi.Database;
using breakincycleapi.Database.Models;

namespace breakincycleapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProgressesController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProgressesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProgresses()
    {
        var progresses = await _context.Progresses.ToListAsync();
        return Ok(progresses);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProgressById(long id)
    {
        var progress = await _context.Progresses.FindAsync(id);
        if (progress == null) return NotFound(new { message = "Progress record not found." });

        return Ok(progress);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProgress([FromBody] Progress progress)
    {
        if (progress == null) return BadRequest(new { message = "Invalid data." });
        
        progress.LastUpdated = DateTime.UtcNow;

        _context.Progresses.Add(progress);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProgressById), new { id = progress.ProgressId }, progress);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProgress(long id, [FromBody] Progress updatedProgress)
    {
        if (id != updatedProgress.ProgressId) return BadRequest(new { message = "ID mismatch." });

        var existingProgress = await _context.Progresses.FindAsync(id);
        if (existingProgress == null) return NotFound(new { message = "Progress record not found." });

        // Update progress percentage
        existingProgress.ProgressPercentage = updatedProgress.ProgressPercentage;
        existingProgress.LastUpdated = DateTime.UtcNow;

        _context.Progresses.Update(existingProgress);
        await _context.SaveChangesAsync();

        return Ok(existingProgress);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProgress(long id)
    {
        var progress = await _context.Progresses.FindAsync(id);
        if (progress == null) return NotFound(new { message = "Progress mapping not found." });

        _context.Progresses.Remove(progress);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Progress mapped deleted successfully." });
    }
}