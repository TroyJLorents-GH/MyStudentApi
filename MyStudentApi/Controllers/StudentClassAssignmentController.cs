using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyStudentApi.Data;
using MyStudentApi.DTO;
using MyStudentApi.Models;
using MyStudentApi.Helpers;
using System.Threading.Tasks;
using System.Formats.Asn1;
using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using System.IO;


namespace MyStudentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentClassAssignmentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentClassAssignmentController(AppDbContext context)
        {
            _context = context;
        }

        // GET all
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentClassAssignment>>> GetAssignments()
            => await _context.StudentClassAssignments.ToListAsync();

        // GET by id
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentClassAssignment>> GetAssignment(int id)
        {
            var a = await _context.StudentClassAssignments.FindAsync(id);
            if (a == null) return NotFound();
            return a;
        }

        [HttpGet("totalhours/{studentId}")]
        public async Task<ActionResult<int>> GetTotalAssignedHours(int studentId)
        {
            // Sum the WeeklyHours for assignments matching the studentId.
            // If there are no assignments, default to 0.
            var totalHours = await _context.StudentClassAssignments
                .Where(a => a.Student_ID == studentId)
                .SumAsync(a => (int?)a.WeeklyHours) ?? 0;
            return totalHours;
        }

        // POST: api/StudentClassAssignment
        [HttpPost]
        public async Task<ActionResult<StudentClassAssignment>> CreateAssignment([FromBody] StudentClassAssignment assignment)
        {
            // You could add validation here if needed.

            _context.StudentClassAssignments.Add(assignment);
            await _context.SaveChangesAsync();

            // Returns a 201 Created with the location header pointing to the new assignment.
            return CreatedAtAction(nameof(GetAssignment), new { id = assignment.Id }, assignment);
        }

        // PUT: api/StudentClassAssignment/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAssignment(int id, [FromBody] StudentAssignmentUpdateDto dto)
        {
            var existing = await _context.StudentClassAssignments.FindAsync(id);
            if (existing == null) return NotFound();

            // Only update what the UI is allowed to modify
            existing.Position_Number = dto.Position_Number;
            existing.I9_Sent = dto.I9_Sent;
            existing.SSN_Sent = dto.SSN_Sent;
            existing.Offer_Sent = dto.Offer_Sent;
            existing.Offer_Signed = dto.Offer_Signed;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadAssignmentsFromCsv([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty or missing.");

            var assignments = new List<StudentClassAssignment>();
            var now = DateTime.UtcNow;

            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
            {
                TrimOptions = TrimOptions.Trim,
                IgnoreBlankLines = true,
                HeaderValidated = null,
                MissingFieldFound = null
            }))
            {
                var records = csv.GetRecords<StudentClassAssignment>().ToList();

                foreach (var record in records)
                {
                    record.CreatedAt = now;
                    record.Term = "2254";

                    record.AcadCareer = AssignmentUtils.InferAcadCareer(record);
                    record.CostCenterKey = AssignmentUtils.ComputeCostCenterKey(record);
                    record.Compensation = AssignmentUtils.CalculateCompensation(record);


                    // GPA logic for bulk upload
                    if (record.FultonFellow?.ToLower() == "yes")
                    {
                        var student = await _context.StudentLookups
                            .FirstOrDefaultAsync(s => s.Student_ID == record.Student_ID);

                        if (student != null)
                        {
                            record.cur_gpa = student.Current_GPA;
                            record.cum_gpa = student.Cumulative_GPA;
                        }
                        else
                        {
                            // Optional: if there is no GPA info then puts 0
                            record.cur_gpa = '0';
                            record.cum_gpa = '0';
                        }
                    }

                    assignments.Add(record);
                }
            }

            _context.StudentClassAssignments.AddRange(assignments);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"{assignments.Count} records uploaded successfully." });
        }

         [HttpGet("template")]
         public IActionResult DownloadTemplate()
            {
              var headers = new[]
                {
                    "Position",
                    "FultonFellow",
                    "WeeklyHours",
                    "Student_ID",
                    "First_Name",
                    "Last_Name",
                    "Email",
                    "EducationLevel",
                    "Subject",
                    "CatalogNum",
                    "InstructorFirstName",
                    "InstructorLastName",
                    "ClassSession",
                    "ClassNum",
                    "Term",
                    "Location",
                    "Campus"
                };

                // Build CSV content
                var csv = new StringBuilder();
                csv.AppendLine(string.Join(",", headers));

                // Add an example row for guidance
                var exampleRow = string.Join(",", new[]
                {
                    "IA/TA/Grader",
                    "Yes/No",
                    "5/10/15/20",
                    "10 digit Id",
                    "Name",
                    "Last Name",
                    "email@xxx.edu",
                    "MS/PHD",
                    "CSE/CIS/IEE",
                    "100-700",
                    "First Name",
                    "Last Name",
                    "A/B/C",
                    "12345",
                    "2254",
                    "TEMPE",
                    "TEMPE"
                });
                csv.AppendLine(exampleRow);

                var bytes = Encoding.UTF8.GetBytes(csv.ToString());
                return File(bytes, "text/csv", "BulkUploadTemplate.csv");
         }
    }
}
