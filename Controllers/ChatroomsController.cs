using breakincycleapi.Database;
using breakincycleapi.Database.Models;
using breakincycleapi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;

namespace breakincycleapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatroomsController : ControllerBase
{
    private readonly IChatroomService _chatroomService;

    public ChatroomsController(IChatroomService chatroomService)
    {
        _chatroomService = chatroomService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllChatrooms()
    {
        var result = await _chatroomService.GetAllRoomsAsync();
        return Ok(result);          
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetChatroomById(Guid id) // Roomid is Guid
    {
        var result = await _chatroomService.GetRoomByIdAsync(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateChatroom([FromBody] DTO_s.ChatroomCreateDto chatroom)
    {
        var newId = await _chatroomService.CreateRoomAsync(chatroom);
        return CreatedAtAction(nameof(GetChatroomById), new { id = newId }, new { id = newId });
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateChatroom(Guid id, [FromBody] DTO_s.ChatroomUpdateDto chatroom)
    {
         if (chatroom == null) 
            return BadRequest(new {message ="Data is required"});
        var success = await _chatroomService.UpdateRoomAsync(id, chatroom);
        if (!success)
            return NotFound(new { message = $"Chatroom with ID {id} not found." });
            return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteChatroom(Guid id)
    {
        var deleted = await _chatroomService.DeleteRoomAsync(id);


        if (!deleted) return NotFound(new { message = "Chatroom not found." });

        return Ok(new { message = "Chatroom deleted successfully." });
    }
}