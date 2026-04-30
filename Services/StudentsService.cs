using breakincycleapi.Database;
using breakincycleapi.Database.Models;
using breakincycleapi.DTO_s;
using Microsoft.EntityFrameworkCore; 

namespace breakincycleapi.Services
{
    public class StudentsService : IStudentsService
    {
        private readonly AppDbContext _context;

        public StudentsService(AppDbContext context)
        {
            _context = context;
        }

        // Renamed from GetAllRoomsAsync to be accurate
        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student?> GetStudentByIdAsync(Guid id)
        {
            return await _context.Students.FindAsync(id);
        }

        public async Task<Guid> CreateStudentAsync(StudentDto studentDTO)
        {
            var student = new Student
            {
                StudentId = Guid.NewGuid(),
                Name = studentDTO.Name,
                Email = studentDTO.Email,
                Phonenumber = studentDTO.PhoneNumber, 
                Location = studentDTO.Location,
                Createdat = DateTime.UtcNow,
                Lastactive = DateTime.UtcNow
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student.StudentId;
        }

        public async Task<bool> UpdateStudentAsync(Guid id, StudentDto studentDTO)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return false;

            student.Name = studentDTO.Name;
            student.Email = studentDTO.Email;
            student.Phonenumber = studentDTO.PhoneNumber;
            student.Location = studentDTO.Location;
            student.Lastactive = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteStudentAsync(Guid id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return false;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}