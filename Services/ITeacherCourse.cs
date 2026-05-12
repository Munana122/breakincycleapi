using breakincycleapi.DTO_s;

namespace breakincycleapi.Services
{
    public interface ITeacherCourse
    {
        Task<IEnumerable<object>> GetAllTeacherCourseAsync();
        Task<object> GetTeacherCourseByIdAsync(long id);
        Task<Guid> CreateTeacherCourseAsync(TeacherCourseCreateDto dto);
        Task<Guid> DeleteTeacherCourseByIdAsync(long id);
    }
}
