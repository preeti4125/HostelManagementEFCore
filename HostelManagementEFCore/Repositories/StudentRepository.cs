using Microsoft.EntityFrameworkCore;
using HostelManagementEFCore.Data;
using HostelManagementEFCore.Models;

namespace HostelManagementEFCore.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly HostelDbContext _context;

        public StudentRepository(HostelDbContext context)
        {
            _context = context;
        }

        // Uses Include to eager load the assigned Room
        public async Task<List<Student>> GetAllStudentsWithRoomAsync()
        {
            return await _context.Students
                .Include(s => s.Room)
                .ToListAsync();
        }

        public async Task<Student?> GetStudentByIdWithRoomAsync(int studentId)
        {
            return await _context.Students
                .Include(s => s.Room)
                .FirstOrDefaultAsync(s => s.StudentId == studentId);
        }

        public async Task AddStudentAsync(Student student)
        {
            await _context.Students.AddAsync(student);
        }

        public async Task RemoveStudentAsync(Student student)
        {
            _context.Students.Remove(student);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}