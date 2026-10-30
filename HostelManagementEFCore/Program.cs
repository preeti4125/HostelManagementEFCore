using HostelManagementEFCore.Data;
using HostelManagementEFCore.Repositories;
using HostelManagementEFCore.Services;

using var db = new HostelDbContext();

// Repositories
IRoomRepository roomRepo = new RoomRepository(db);
IStudentRepository studentRepo = new StudentRepository(db);
IAuditLogRepository auditRepo = new AuditLogRepository(db);

// Service
var service = new HostelService(roomRepo, studentRepo, auditRepo);

bool running = true;

while (running)
{
    Console.WriteLine("\n==============================");
    Console.WriteLine("    HOSTEL MANAGEMENT SYSTEM  ");
    Console.WriteLine("==============================");
    Console.WriteLine("1. Add Room");
    Console.WriteLine("2. Add Student to Room");
    Console.WriteLine("3. View All Rooms & Students");
    Console.WriteLine("4. View Audit Logs");
    Console.WriteLine("5. Exit");
    Console.Write("\nEnter choice: ");

    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            Console.Write("Enter Room Number (e.g. A-101): ");
            var roomNo = Console.ReadLine() ?? "";

            Console.Write("Enter Room Capacity: ");
            int.TryParse(Console.ReadLine(), out int cap);

            await service.AddRoomAsync(roomNo, cap);
            break;

        case "2":
            Console.Write("Enter Student Name: ");
            var name = Console.ReadLine() ?? "";

            Console.Write("Enter Student Phone: ");
            var phone = Console.ReadLine() ?? "";

            Console.Write("Enter Room ID to Assign: ");
            int.TryParse(Console.ReadLine(), out int rId);

            await service.AddStudentAsync(name, phone, rId);
            break;

        case "3":
            await service.DisplayRoomsWithStudentsAsync();
            break;

        case "4":
            await service.DisplayAuditLogsAsync();
            break;

        case "5":
            running = false;
            Console.WriteLine("Exiting Hostel Management System. Goodbye!");
            break;

        default:
            Console.WriteLine("Invalid option. Please choose between 1 and 5.");
            break;
    }
}