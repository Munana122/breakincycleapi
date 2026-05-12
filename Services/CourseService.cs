using breakincycleapi.DTO_s;
using breakincycleapi.Database;
using breakincycleapi.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace breakincycleapi.Services
{
    public class CourseService : ICourceService
    {
        private readonly AppDbContext _context;
        public CourseService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Object>> GetAllCoursesAsync()
        {
            return await _context.Courses.ToListAsync<Object>();
        }

        public async Task<Object> GetCourseByIdAsync(Guid id)
        {
            return await _context.Courses.FindAsync(id);
        }

        public async Task<Guid> CreateCourseAsync(CourseCreateDto courseCreateDto)
        {
            var newCourse = new Course
            {
                CourseId = Guid.NewGuid(),
                Name = courseCreateDto.Name,
                Description = courseCreateDto.Description ?? string.Empty,
                Createdat = DateTime.UtcNow,
                Lastactive = DateTime.UtcNow
            };
            _context.Courses.Add(newCourse);
            await _context.SaveChangesAsync();
            return newCourse.CourseId;
        }

        public async Task<Guid> UpdateCourseAsync(CourseUpdateDTO courseUpdateDto)
        {
            var existingCourse = await _context.Courses.FindAsync(courseUpdateDto.CourseId);
            if (existingCourse == null) return Guid.Empty;
            existingCourse.Name = courseUpdateDto.Name;
            existingCourse.Description = courseUpdateDto.Description ?? existingCourse.Description;
            existingCourse.Lastactive = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return existingCourse.CourseId;
        }

        public async Task<bool> DeleteCourseAsync(Guid id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return false;
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
