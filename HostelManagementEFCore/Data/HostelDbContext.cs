using Microsoft.EntityFrameworkCore;
using HostelManagementEFCore.Models;

namespace HostelManagementEFCore.Data
{
    public class HostelDbContext : DbContext
    {
        public DbSet<Room> Rooms => Set<Room>();
        public DbSet<Student> Students => Set<Student>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Uses SQL Server LocalDB default instance
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\mssqllocaldb;Database=HostelDb;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure 1-to-Many: One Room has many Students
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Room)
                .WithMany(r => r.Students)
                .HasForeignKey(s => s.RoomId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}