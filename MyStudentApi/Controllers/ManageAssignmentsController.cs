using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyStudentApi.Data;
using MyStudentApi.Models;
using MyStudentApi.Helpers;

namespace MyStudentApi.Controllers
{
    [ApiController]
    [Route("api/manage-assignments")]
    public class ManageAssignmentsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ManageAssignmentsController(AppDbContext db)
        {
            _db = db;
        }

        // Inline DTO so there’s no extra namespace/file to create
        public class ManageAssignmentUpdateDTO
        {
            public int? WeeklyHours { get; set; }
            public string? Position { get; set; }
            public string? Subject { get; set; }
            public int? CatalogNum { get; set; }
            public string? ClassSession { get; set; }
            public string? ClassNum { get; set; } // your model uses string ClassNum
            public string? FultonFellow { get; set; }
        }

        // ========= GET /api/manage-assignments/by-instructor/{instructor_id} =========
        [HttpGet("by-instructor/{instructorId:int}")]
        public async Task<IActionResult> GetByInstructor(int instructorId)
        {
            // Only include assignments where Instructor_Edit is not 'Y' or 'D' (or is null/None)
            var list = await _db.StudentClassAssignments
                .Where(a => a.InstructorID == instructorId &&
                           (a.Instructor_Edit == null || (a.Instructor_Edit != "Y" && a.Instructor_Edit != "D")))
                .ToListAsync();

            var result = list.Select(a => new
            {
                Id = a.Id,
                Student_ID = a.Student_ID,
                ASUrite = a.ASUrite,
                First_Name = a.First_Name,
                Last_Name = a.Last_Name,
                Position = a.Position,
                WeeklyHours = a.WeeklyHours,
                FultonFellow = a.FultonFellow,
                Email = a.Email,
                EducationLevel = a.EducationLevel,
                Subject = a.Subject,
                CatalogNum = a.CatalogNum,
                ClassSession = a.ClassSession,
                ClassNum = a.ClassNum,
                Term = a.Term,
                InstructorFirstName = a.InstructorFirstName,
                InstructorLastName = a.InstructorLastName,
                // InstructorID omitted because it’s not in your model
                Location = a.Location,
                Campus = a.Campus,
                AcadCareer = a.AcadCareer
            });

            return Ok(result);
        }

        // ========= PUT /api/manage-assignments/{assignment_id} =========
        [HttpPut("{assignmentId:int}")]
        public async Task<IActionResult> UpdateAssignment(int assignmentId, [FromBody] ManageAssignmentUpdateDTO update)
        {
            var a = await _db.StudentClassAssignments.FirstOrDefaultAsync(x => x.Id == assignmentId);
            if (a == null)
                return NotFound(new { detail = "Assignment not found" });

            var changed = new HashSet<string>();

            void Apply<T>(string name, T? value, Action<T> setter) where T : struct
            {
                if (value is null) return;
                setter(value.Value);
                changed.Add(name);
            }

            void ApplyRef<T>(string name, T? value, Action<T> setter) where T : class
            {
                if (value is null) return;
                setter(value);
                changed.Add(name);
            }

            Apply(nameof(update.WeeklyHours), update.WeeklyHours, v => a.WeeklyHours = v);
            ApplyRef(nameof(update.Position), update.Position, v => a.Position = v);
            ApplyRef(nameof(update.Subject), update.Subject, v => a.Subject = v);
            Apply(nameof(update.CatalogNum), update.CatalogNum, v => a.CatalogNum = v);
            ApplyRef(nameof(update.ClassSession), update.ClassSession, v => a.ClassSession = v);
            ApplyRef(nameof(update.ClassNum), update.ClassNum, v => a.ClassNum = v);
            ApplyRef(nameof(update.FultonFellow), update.FultonFellow, v => a.FultonFellow = v);

            // If CatalogNum changed, infer AcadCareer
            if (changed.Contains(nameof(update.CatalogNum)))
            {
                a.AcadCareer = AssignmentUtils.InferAcadCareer(a);
                changed.Add(nameof(a.AcadCareer));
            }

            // Recompute Compensation if relevant fields changed
            var compensationFields = new[] { "Position", "WeeklyHours", "EducationLevel", "FultonFellow", "ClassSession" };
            if (compensationFields.Any(changed.Contains))
            {
                a.Compensation = AssignmentUtils.CalculateCompensation(a);
            }

            // Recompute CostCenterKey if relevant fields changed (or AcadCareer changed)
            var costCenterFields = new[] { "Position", "Location", "Campus", "AcadCareer" };
            if (costCenterFields.Any(changed.Contains))
            {
                a.CostCenterKey = AssignmentUtils.ComputeCostCenterKey(a);
            }

            await _db.SaveChangesAsync();

            var response = new
            {
                message = "Assignment updated successfully.",
                assignment = new
                {
                    Id = a.Id,
                    WeeklyHours = a.WeeklyHours,
                    Position = a.Position,
                    FultonFellow = a.FultonFellow,
                    Subject = a.Subject,
                    CatalogNum = a.CatalogNum,
                    ClassSession = a.ClassSession,
                    ClassNum = a.ClassNum,
                    Compensation = a.Compensation,
                    CostCenterKey = a.CostCenterKey
                }
            };

            return Ok(response);
        }
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        