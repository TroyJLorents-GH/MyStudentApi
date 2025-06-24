using Microsoft.EntityFrameworkCore;
using MyStudentApi.Models;
using YourNamespace.Models;

namespace MyStudentApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // This DbSet will allow you to query and save instances of Student.
        public DbSet<ClassLookup> ClassLookups { get; set; }
        public DbSet<ClassSchedule2254> ClassSchedule2254 { get; set; }
        public DbSet<StudentClassAssignment> StudentClassAssignments { get; set; }
        public DbSet<MastersIAGraderApplication2254> MastersIAGraderApplications { get; set; }
        public DbSet<StudentLookup> StudentLookups { get; set; }



        // (Optional) If you need additional configuration via the Fluent API, override OnModelCreating:
        /*
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>(entity =>
            {
                // This is redundant if you already used the [Table] attribute.
                entity.ToTable("SPR25_Hiring", "dbo");

                // Configure the primary key if needed (redundant with [Key] attribute):
                entity.HasKey(e => e.PK);
            });
        }
        */
    }
}
