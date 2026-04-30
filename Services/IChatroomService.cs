using breakincycleapi.DTO_s;

namespace breakincycleapi.Services
{
    public interface IChatroomService
    {
        Task<IEnumerable<object>> GetAllRoomsAsync();
        Task<object?> GetRoomByIdAsync(Guid id);
        Task<Guid> CreateRoomAsync(ChatroomCreateDto dto);
        Task<bool> UpdateRoomAsync(Guid id, ChatroomUpdateDto dto);
        Task<bool> DeleteRoomAsync(Guid id);
    }
}
