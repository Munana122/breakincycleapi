using breakincycleapi.Database;
using breakincycleapi.Database.Models;
using breakincycleapi.DTO_s;
using Microsoft.EntityFrameworkCore;

namespace breakincycleapi.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly AppDbContext _context;

        public TeacherService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Teacher>> GetAllTeachersAsync()
        {
            return await _context.Teachers.ToListAsync();
        }

        public async Task<Teacher?> GetTeacherByIdAsync(Guid id)
        {
            return await _context.Teachers.FindAsync(id);
        }

        public async Task<Guid> CreateTeacherAsync(TeacherCreateDto dto)
        {
            var teacher = new Teacher
            {
                TeacherId = Guid.NewGuid(),
                Name = dto.Name,
                Email = dto.Email,
                Coursename = dto.Coursename,
                Phonenumber = dto.PhoneNumber,
                Location = dto.Location,
                Createdat = DateTime.UtcNow,
                Lastactive = DateTime.UtcNow
            };

            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();
            return teacher.TeacherId;
        }

        public async Task<bool> UpdateTeacherAsync(Guid id, TeacherUpdateDTO dto)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null) return false;

            teacher.Name = dto.Name;
            teacher.Email = dto.Email;
            teacher.Phonenumber = dto.Phonenumber;
            teacher.Coursename = dto.Coursename;
            teacher.Lastactive = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTeacherAsync(Guid id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null) return false;

            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}