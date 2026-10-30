using HostelManagementEFCore.Models;

namespace HostelManagementEFCore.Repositories
{
    public interface IRoomRepository
    {
        Task<List<Room>> GetAllRoomsWithStudentsAsync();
        Task<Room?> GetRoomByIdWithStudentsAsync(int roomId);
        Task<Room?> GetRoomByIdAsync(int roomId);
        Task AddRoomAsync(Room room);
        Task SaveChangesAsync();
    }
}