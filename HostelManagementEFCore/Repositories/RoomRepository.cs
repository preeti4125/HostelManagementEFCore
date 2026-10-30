using Microsoft.EntityFrameworkCore;
using HostelManagementEFCore.Data;
using HostelManagementEFCore.Models;

namespace HostelManagementEFCore.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly HostelDbContext _context;

        public RoomRepository(HostelDbContext context)
        {
            _context = context;
        }

        // Uses Include to eager load related Students
        public async Task<List<Room>> GetAllRoomsWithStudentsAsync()
        {
            return await _context.Rooms
                .Include(r => r.Students)
                .ToListAsync();
        }

        // Find single room with student details
        public async Task<Room?> GetRoomByIdWithStudentsAsync(int roomId)
        {
            return await _context.Rooms
                .Include(r => r.Students)
                .FirstOrDefaultAsync(r => r.RoomId == roomId);
        }

        // Direct lookup without related data
        public async Task<Room?> GetRoomByIdAsync(int roomId)
        {
            return await _context.Rooms.FindAsync(roomId);
        }

        // Add entity using DbContext method
        public async Task AddRoomAsync(Room room)
        {
            await _context.Rooms.AddAsync(room);
        }

        // Commit changes to SQL Server
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}