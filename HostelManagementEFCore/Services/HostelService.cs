using Microsoft.EntityFrameworkCore;
using HostelManagementEFCore.Data;
using HostelManagementEFCore.Models;

namespace HostelManagementEFCore.Services
{
    public class HostelService
    {
        private readonly HostelDbContext _db;

        public HostelService(HostelDbContext db)
        {
            _db = db;
        }

        // Add a new Room asynchronously
        public async Task AddRoomAsync(string roomNumber, int capacity)
        {
            var room = new Room
            {
                RoomNumber = roomNumber,
                Capacity = capacity
            };

            await _db.Rooms.AddAsync(room);
            await _db.SaveChangesAsync();

            Console.WriteLine($"\n[Success] Room '{roomNumber}' created with ID: {room.RoomId}");
        }

        // Add a Student and assign to a Room asynchronously
        public async Task AddStudentAsync(string name, string phone, int roomId)
        {
            var room = await _db.Rooms.FindAsync(roomId);
            if (room == null)
            {
                Console.WriteLine($"\n[Error] Room with ID {roomId} does not exist.");
                return;
            }

            var student = new Student
            {
                Name = name,
                Phone = phone,
                RoomId = roomId
            };

            await _db.Students.AddAsync(student);
            await _db.SaveChangesAsync();

            Console.WriteLine($"\n[Success] Student '{name}' assigned to Room {room.RoomNumber}!");
        }

        // Display all Rooms and their allocated Students using Eager Loading (.Include)
        public async Task DisplayRoomsWithStudentsAsync()
        {
            var rooms = await _db.Rooms
                .Include(r => r.Students)
                .ToListAsync();

            if (rooms.Count == 0)
            {
                Console.WriteLine("\nNo rooms found in the system.");
                return;
            }

            Console.WriteLine("\n==============================");
            Console.WriteLine("    HOSTEL ALLOCATION LIST    ");
            Console.WriteLine("==============================");

            foreach (var r in rooms)
            {
                Console.WriteLine($"\nRoom ID: {r.RoomId} | Number: {r.RoomNumber} | Capacity: {r.Capacity}");

                if (r.Students.Count == 0)
                {
                    Console.WriteLine("  (No students enrolled)");
                }
                else
                {
                    foreach (var s in r.Students)
                    {
                        Console.WriteLine($"  -> [Student ID: {s.StudentId}] {s.Name} | Phone: {s.Phone}");
                    }
                }
            }
        }
    }
}