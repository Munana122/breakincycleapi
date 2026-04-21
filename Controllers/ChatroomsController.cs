using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using breakincycleapi.Database;
using breakincycleapi.Database.Models;

namespace breakincycleapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatroomsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ChatroomsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllChatrooms()
    {
        var chatrooms = await _context.Chatrooms
            .Select(c => new
            {
                c.Roomid,
                name = c.Name,
                c.Userid,
                c.JoinedAt,
                description = c.Description,
                messages = c.Messages.Select(m => new
                {
                    m.MessageId,
                    m.Roomid,
                    m.UserId,
                    m.Name,
                    m.Message1,
                    m.Createdat,
                    Room = c.Name,
                    user = m.User.Name
                }).ToList()
            })
            .FirstOrDefaultAsync();
        if (chatrooms == null) return NotFound(new { message = "No chatroom found" });
        return Ok(chatrooms);           
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetChatroomById(Guid id) // Roomid is Guid
    {
        var chatrooms = await _context.Chatrooms
              .Where(c => c.Roomid == id)
              .Select(c => new
              {
                  c.Roomid,
                  name = c.Name,
                  c.Userid,
                  c.JoinedAt,
                  description = c.Description,
                  messages = c.Messages.Select(m => new
                  {
                      m.MessageId,
                      m.Roomid,
                      m.UserId,
                      m.Name,
                      m.Message1,
                      m.Createdat,
                      Room = c.Name,
                      user = m.User.Name
                  }).ToList()
              })
              .FirstOrDefaultAsync();
        if (chatrooms == null) return NotFound(new { message = "No chatroom found" });
        return Ok(chatrooms);
    }

    [HttpPost]
    public async Task<IActionResult> CreateChatroom([FromBody] DTO_s.ChatroomCreateDto chatroom)
    {
        if (chatroom == null) return BadRequest(new { message = "Invalid data." });
        
        var newChatroom = new Chatroom
        {
            Roomid = Guid.NewGuid(),
            JoinedAt = DateTime.UtcNow,
            Name = chatroom.Name,
            Description = chatroom.Description
        };

        _context.Chatrooms.Add(newChatroom);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetChatroomById), new { id = newChatroom.Roomid }, newChatroom);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteChatroom(Guid id)
    {
        var chatroom = await _context.Chatrooms.FindAsync(id);
        if (chatroom == null) return NotFound(new { message = "Chatroom not found." });

        _context.Chatrooms.Remove(chatroom);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Chatroom deleted successfully." });
    }
}