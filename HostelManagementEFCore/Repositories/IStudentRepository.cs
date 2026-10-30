using HostelManagementEFCore.Models;

namespace HostelManagementEFCore.Repositories
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetAllStudentsWithRoomAsync();
        Task<Student?> GetStudentByIdWithRoomAsync(int studentId);
        Task AddStudentAsync(Student student);
        Task RemoveStudentAsync(Student student);
        Task SaveChangesAsync();
    }
}