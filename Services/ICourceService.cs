using breakincycleapi.DTO_s;

namespace breakincycleapi.Services
{
    public interface ICourceService
    {
        Task<IEnumerable<Object>> GetAllCoursesAsync();
        Task<Object> GetCourseByIdAsync(Guid id);
        Task<Guid> CreateCourseAsync(CourseCreateDto courseCreateDto);
        Task<Guid> UpdateCourseAsync(CourseUpdateDTO courseUpdateDto);
        Task<bool> DeleteCourseAsync(Guid id);
    }
}
