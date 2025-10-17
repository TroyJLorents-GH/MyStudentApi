using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyStudentApi.Data;
using MyStudentApi.Models;

namespace MyStudentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacultyController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FacultyController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Faculty/student-assignments
        // Read-only view of all student assignments for faculty dashboard
        [HttpGet("student-assignments")]
        public async Task<IActionResult> GetFacultyAssignments()
        {
            var rows = await _context.StudentClassAssignments.ToListAsync();

            var safe = rows.Select(r => new
            {
                Id = r.Id,
                Student_ID = r.Student_ID,
                ASUrite = r.ASUrite,
                Position = r.Position,
                WeeklyHours = r.WeeklyHours,
                FultonFellow = r.FultonFellow,
                Email = r.Email,
                EducationLevel = r.EducationLevel,
                Subject = r.Subject,
                CatalogNum = r.CatalogNum,
                ClassSession = r.ClassSession,
                ClassNum = r.ClassNum,
                Term = r.Term,
                InstructorFirstName = r.InstructorFirstName,
                InstructorLastName = r.InstructorLastName,
                Location = r.Location,
                Campus = r.Campus,
                AcadCareer = r.AcadCareer,
                First_Name = r.First_Name,
                Last_Name = r.Last_Name,
                cum_gpa = r.cum_gpa,
                cur_gpa = r.cur_gpa
            }).ToList();

            return Ok(safe);
        }
    }
}
