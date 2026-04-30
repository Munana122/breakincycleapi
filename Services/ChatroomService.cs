using breakincycleapi.Database;
using breakincycleapi.Database.Models;
using breakincycleapi.DTO_s;
using Microsoft.EntityFrameworkCore;

namespace breakincycleapi.Services
{
    public class ChatroomService : IChatroomService
    {
        private readonly AppDbContext _context;

        public ChatroomService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> GetAllRoomsAsync()
        {
            return await _context.Chatrooms
                .Select(c => new {
                    c.Roomid,
                    name = c.Name,
                    c.Description,
                    messages = c.Messages.Count // Example: just return count for list view
                }).ToListAsync();
        }

        public async Task<Guid> CreateRoomAsync(ChatroomCreateDto dto)
        {
            var newChatroom = new Chatroom
            {
                Roomid = Guid.NewGuid(),
                JoinedAt = DateTime.UtcNow,
                Name = dto.Name,
                Description = dto.Description,
                Userid = Guid.Empty
            };

            _context.Chatrooms.Add(newChatroom);
            await _context.SaveChangesAsync();
            return newChatroom.Roomid;
        }
        public async Task<bool> UpdateRoomAsync(Guid id, ChatroomUpdateDto dto)
        {
            var room = await _context.Chatrooms.FindAsync(id);
            if (room == null) return false;

            if (dto.Name != null) room.Name = dto.Name;
            if (dto.Description != null) room.Description = dto.Description;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<object?> GetRoomByIdAsync(Guid id)
        {
            // Move your complex .Select() logic here
            return await _context.Chatrooms
                .Where(c => c.Roomid == id)
                .Select(c => new { c.Roomid, name = c.Name }) // Shortened for example
                .FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteRoomAsync(Guid id)
        {
            var room = await _context.Chatrooms.FindAsync(id);
            if (room == null) return false;

            _context.Chatrooms.Remove(room);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}