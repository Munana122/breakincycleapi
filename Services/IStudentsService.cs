using breakincycleapi.Database.Models;
using breakincycleapi.DTO_s;

namespace breakincycleapi.Services
{
    public interface IStudentsService
    {
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task<Student?> GetStudentByIdAsync(Guid id);
        Task<Guid> CreateStudentAsync(StudentDto studentDto);
        Task<bool> UpdateStudentAsync(Guid id, StudentDto studentDto);
        Task<bool> DeleteStudentAsync(Guid id);
    }
}
