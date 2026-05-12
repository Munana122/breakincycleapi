using breakincycleapi.Database;
using breakincycleapi.Database.Models;
using breakincycleapi.DTO_s;
using breakincycleapi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace breakincycleapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{
    private readonly IMessageService _messageService;

    public MessagesController(IMessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMessages()
    {
       var result =await _messageService.GetAllMessagesAsync();
       return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMessageById(long id) 
    {
        var result = await _messageService.GetMessageByIdAsync(id);
        if (result == null) { return NotFound(new { message = "Message not found." });}
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMessage([FromBody] MessageCreateDto dto)
    {
        if (dto == null)
            return BadRequest(new { message = "Message data is required." });
        var newId = await _messageService.CreateMessageAsync(dto);

        return CreatedAtAction(nameof(GetMessageById), new { id = newId }, new { id = newId });
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMessage(long id)
    {
        var success = await _messageService.DeleteMessageAsync(id);
        return Ok(new { message = "Message deleted successfully." });
    }
}