using breakincycleapi.Database;
using breakincycleapi.Database.Models;
using breakincycleapi.DTO_s;
using Microsoft.EntityFrameworkCore;
using breakincycleapi.Services;

namespace breakincycleapi.Services
{
    public class MessageService : IMessageService
    {
        private readonly AppDbContext _context;
        public MessageService(AppDbContext context) 
        {
            _context = context;
        }
        public async Task<IEnumerable<Object>> GetAllMessagesAsync()
        {
            return await _context.Messages
                .Include(m => m.Room)
                .Include(m => m.User)
                .Select(m => new
                {
                    m.MessageId,
                    m.Roomid,
                    m.UserId,
                    m.Name,
                    m.Message1,
                    m.Createdat,
                    Room = m.Room != null ? m.Room.Name : null,
                    User = m.User != null ? m.User.Name : null
                })
                .ToListAsync<Object>();
        }
        public async Task<Object?> GetMessageByIdAsync(long id)
        {
            return await _context.Messages
                .Include(m => m.Room)
                .Include(m => m.User)
                .Where(m => m.MessageId == id)
                .Select(m => new
                {
                    m.MessageId,
                    m.Roomid,
                    m.UserId,
                    m.Name,
                    m.Message1,
                    m.Createdat,
                    Room = m.Room != null ? m.Room.Name : null,
                    User = m.User != null ? m.User.Name : null
                })
                .FirstOrDefaultAsync();
        }
        public async Task<long> CreateMessageAsync(MessageCreateDto messageDto)
        {
            var newMessage = new Message
            {
                UserId = Guid.NewGuid(),
                Roomid = messageDto.RoomId,
                Name = messageDto.Name,
                Message1 = messageDto.Message,
                Createdat = DateTime.UtcNow
            };
            var room = await _context.Chatrooms.FindAsync(messageDto.RoomId); 
            if (room != null)
            {
                room.MessageSenderName = messageDto.Name;
                room.MessageContent = messageDto.Message;
            }

            await _context.SaveChangesAsync();
            return newMessage.MessageId;
        }
        public async Task<bool> DeleteMessageAsync(long id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message == null) return false;
            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
