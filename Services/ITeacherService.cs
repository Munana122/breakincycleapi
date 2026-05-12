using breakincycleapi.Database.Models;
using breakincycleapi.DTO_s;

namespace breakincycleapi.Services
{
    public interface ITeacherService
    {
        Task<IEnumerable<Teacher>> GetAllTeachersAsync();
        Task<Teacher?> GetTeacherByIdAsync(Guid id);
        Task<Guid> CreateTeacherAsync(TeacherCreateDto dto);
        Task<bool> UpdateTeacherAsync(Guid id, TeacherUpdateDTO dto);
        Task<bool> DeleteTeacherAsync(Guid id);
    }
}