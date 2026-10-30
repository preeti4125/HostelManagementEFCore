using HostelManagementEFCore.Models;
using HostelManagementEFCore.Repositories;

namespace HostelManagementEFCore.Services
{
    public class HostelService
    {
        private readonly IRoomRepository _roomRepo;
        private readonly IStudentRepository _studentRepo;
        private readonly IAuditLogRepository _auditRepo; // Injected audit repository

        public HostelService(
            IRoomRepository roomRepo,
            IStudentRepository studentRepo,
            IAuditLogRepository auditRepo)
        {
            _roomRepo = roomRepo;
            _studentRepo = studentRepo;
            _auditRepo = auditRepo;
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

            // Record Audit Log
            await _auditRepo.LogAsync("CREATE", "Room", $"Added Room {roomNumber} with Capacity {capacity} (ID: {room.RoomId})");
            await _auditRepo.SaveChangesAsync();

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

            // Record Audit Log
            await _auditRepo.LogAsync("CREATE", "Student", $"Assigned Student '{name}' (Phone: {phone}) to Room {room.RoomNumber}");
            await _auditRepo.SaveChangesAsync();

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

        // Display all system audit trails
        public async Task DisplayAuditLogsAsync()
        {
            var logs = await _auditRepo.GetAllLogsAsync();

            if (logs.Count == 0)
            {
                Console.WriteLine("\nNo audit logs available.");
                return;
            }

            Console.WriteLine("\n================================================================================");
            Console.WriteLine("                                SYSTEM AUDIT LOGS                               ");
            Console.WriteLine("================================================================================");
            Console.WriteLine($"{"Timestamp (UTC)",-22} | {"Action",-8} | {"Entity",-8} | Details");
            Console.WriteLine(new string('-', 80));

            foreach (var log in logs)
            {
                Console.WriteLine($"{log.Timestamp:yyyy-MM-dd HH:mm:ss} | {log.Action,-8} | {log.EntityName,-8} | {log.Details}");
            }
        }
    }
}