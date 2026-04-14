using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using breakincycleapi.Database;
using breakincycleapi.Database.Models;
using breakincycleapi.DTO_s;

namespace breakincycleapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{
    private readonly AppDbContext _context;

    public MessagesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMessages()
    {
        var messages = await _context.Messages.ToListAsync();
        return Ok(messages);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMessageById(long id) // MessageId is 'long'
    {
        var message = await _context.Messages.FindAsync(id);
        if (message == null) return NotFound(new { message = "Message not found." });

        return Ok(message);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMessage([FromBody] MessageCreateDto dto)
    {
        if (dto == null) return BadRequest(new { message = "Invalid data." });

        var message = new Message
        {
            Roomid = dto.RoomId,
            UserId = dto.UserId,
            Name = dto.Name,
            Message1 = dto.Message,
            Createdat = DateTime.UtcNow
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMessageById), new { id = message.MessageId }, message);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMessage(long id, [FromBody] Message updatedMessage)
    {
        if (id != updatedMessage.MessageId) return BadRequest(new { message = "ID mismatch." });

        var existingMessage = await _context.Messages.FindAsync(id);
        if (existingMessage == null) return NotFound(new { message = "Message not found." });

        // According to model, Message text is in Message1 property
        existingMessage.Message1 = updatedMessage.Message1;

        _context.Messages.Update(existingMessage);
        await _context.SaveChangesAsync();

        return Ok(existingMessage);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMessage(long id)
    {
        var message = await _context.Messages.FindAsync(id);
        if (message == null) return NotFound(new { message = "Message not found." });

        _context.Messages.Remove(message);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Message deleted successfully." });
    }
}