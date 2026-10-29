namespace HostelManagementEFCore.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        // Foreign Key pointing to Room
        public int RoomId { get; set; }

        // Navigation property: Student belongs to one room
        public Room Room { get; set; } = null!;
    }
}