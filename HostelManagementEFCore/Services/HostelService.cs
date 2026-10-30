using HostelManagementEFCore.Models;
using HostelManagementEFCore.Repositories;

namespace HostelManagementEFCore.Services
{
    public class HostelService
    {
        private readonly IRoomRepository _roomRepo;
        private readonly IStudentRepository _studentRepo;

        public HostelService(IRoomRepository roomRepo, IStudentRepository studentRepo)
        {
            _roomRepo = roomRepo;
            _studentRepo = studentRepo;
        }

        public async Task AddRoomAsync(string roomNumber, int capacity)
        {
            var room = new Room
            {
                RoomNumber = roomNumber,
                Capacity = capacity
            };

            await _roomRepo.AddRoomAsync(room);
            await _roomRepo.SaveChangesAsync();

            Console.WriteLine($"\n[Success] Room '{roomNumber}' created with ID: {room.RoomId}");
        }

        public async Task AddStudentAsync(string name, string phone, int roomId)
        {
            var room = await _roomRepo.GetRoomByIdWithStudentsAsync(roomId);
            if (room == null)
            {
                Console.WriteLine($"\n[Error] Room with ID {roomId} does not exist.");
                return;
            }

            if (room.Students.Count >= room.Capacity)
            {
                Console.WriteLine($"\n[Warning] Room {room.RoomNumber} is full! (Capacity: {room.Capacity})");
                return;
            }

            var student = new Student
            {
                Name = name,
                Phone = phone,
                RoomId = roomId
            };

            await _studentRepo.AddStudentAsync(student);
            await _studentRepo.SaveChangesAsync();

            Console.WriteLine($"\n[Success] Student '{name}' assigned to Room {room.RoomNumber}!");
        }

        public async Task DisplayRoomsWithStudentsAsync()
        {
            var rooms = await _roomRepo.GetAllRoomsWithStudentsAsync();

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