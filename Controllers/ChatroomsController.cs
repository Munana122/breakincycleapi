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
        var chatrooms = await _context.Chatrooms.ToListAsync();
        return Ok(chatrooms);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetChatroomById(Guid id) // Roomid is Guid
    {
        var chatroom = await _context.Chatrooms.FindAsync(id);
        if (chatroom == null) return NotFound(new { message = "Chatroom not found." });

        return Ok(chatroom);
    }

    [HttpPost]
    public async Task<IActionResult> CreateChatroom([FromBody] Chatroom chatroom)
    {
        if (chatroom == null) return BadRequest(new { message = "Invalid data." });
        
        chatroom.Roomid = Guid.NewGuid();
        chatroom.JoinedAt = DateTime.UtcNow;

        _context.Chatrooms.Add(chatroom);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetChatroomById), new { id = chatroom.Roomid }, chatroom);
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