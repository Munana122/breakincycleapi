using breakincycleapi.DTO_s;
namespace breakincycleapi.Services
{
    public interface IMessageService
    {
        Task<IEnumerable<Object>> GetAllMessagesAsync();
        Task<Object?> GetMessageByIdAsync(long id);
        Task<long> CreateMessageAsync(MessageCreateDto messageDto);
        Task<bool> DeleteMessageAsync(long id);

    }
}
