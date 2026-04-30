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
        var messages = await _context.Messages
            .Select(a => new
            {
                a.MessageId,
                roomid = a.Roomid,
                a.UserId,
                a.Name,
                a.Message1,
                a.Createdat,
                room = a.Room.Name,
                User = a.User.Name
            }).ToListAsync();
        return Ok(messages);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMessageById(long id) // MessageId is 'long'
    {
        var message = await _context.Messages
            .Where(a => a.MessageId == id)
             .Select(a => new
             {
                 a.MessageId,
                 roomid = a.Roomid,
                 a.UserId,
                 a.Name,
                 a.Message1,
                 a.Createdat,
                 room = a.Room.Name,
                 User = a.User.Name
             }).ToListAsync();

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
        var room = await _context.Chatrooms.FindAsync(dto.RoomId);
        if (room != null)
        {
            room.MessageSenderName = dto.Name;
            room.MessageContent = dto.Message;
            // You might also want to update the MessageId in the room table
        }
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMessageById), new { id = message.MessageId }, message);
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