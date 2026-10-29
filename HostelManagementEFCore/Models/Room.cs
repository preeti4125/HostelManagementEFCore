namespace HostelManagementEFCore.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public int Capacity { get; set; }

        // Navigation property: One room can have many students
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}