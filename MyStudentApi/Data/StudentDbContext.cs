using Microsoft.EntityFrameworkCore;
using MyStudentApi.Models;
using YourNamespace.Models;

namespace YourNamespace
{
    public class StudentDbContext : DbContext
    {
        public StudentDbContext(DbContextOptions<StudentDbContext> options) : base(options) { }

        public DbSet<StudentClassAssignment> StudentClassAssignments => Set<StudentClassAssignment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // If you need to map column names exactly:
            // modelBuilder.Entity<StudentClassAssignment>()
            //     .Property(p => p.Student_ID).HasColumnName("Student_ID");

            base.OnModelCreating(modelBuilder);
        }
    }
}
