using breakincycleapi.Database.Models;
using breakincycleapi.DTO_s;

namespace breakincycleapi.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUsersByIdAsync(Guid id);
        Task<Guid> CreateUserAsync(UserCreateDto userDto);
        Task<bool> UpdateUserAsync(Guid id, UserUpdateDto userDto);
        Task<bool> DeleteUserAsync(Guid id);
    }
}
