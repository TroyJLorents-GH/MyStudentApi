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
using YourNamespace.Models;


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

        // GET all (excluding instructor-edited/deleted assignments)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentClassAssignment>>> GetAssignments()
            => await _context.StudentClassAssignments
                .Where(a => a.Instructor_Edit == null)
                .ToListAsync();

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
            // Sum the WeeklyHours for assignments matching the studentId (excluding edited/deleted)
            var totalHours = await _context.StudentClassAssignments
                .Where(a => a.Student_ID == studentId &&
                           (a.Instructor_Edit == null || a.Instructor_Edit == "" || a.Instructor_Edit == "N"))
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

            if (!file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                return BadRequest("CSV file required.");

            var assignments = new List<StudentClassAssignment>();
            var now = DateTime.UtcNow;

            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                TrimOptions = TrimOptions.Trim,
                IgnoreBlankLines = true,
                HeaderValidated = null,
                MissingFieldFound = null
            }))
            {
                csv.Read();
                csv.ReadHeader();
                var headers = csv.HeaderRecord;

                // Detect which template format: old (17 fields) or new (5 fields)
                bool isNewFormat = headers.Any(h => h.Contains("Student_ID (ID number OR ASUrite accepted)"));

                int rowNum = 1;
                while (csv.Read())
                {
                    rowNum++;

                    if (isNewFormat)
                    {
                        // NEW FORMAT: 5 fields - lookup student and class data
                        var position = csv.GetField<string>("Position")?.Trim();
                        var fultonFellow = csv.GetField<string>("FultonFellow")?.Trim();
                        var weeklyHoursStr = csv.GetField<string>("WeeklyHours")?.Trim();
                        var studentIdOrAsurite = csv.GetField<string>("Student_ID (ID number OR ASUrite accepted)")?.Trim();
                        var classNum = csv.GetField<string>("ClassNum")?.Trim();

                        // Validate required fields
                        if (string.IsNullOrWhiteSpace(position))
                            return UnprocessableEntity($"Missing 'Position' in row {rowNum}");
                        if (string.IsNullOrWhiteSpace(weeklyHoursStr))
                            return UnprocessableEntity($"Missing 'WeeklyHours' in row {rowNum}");
                        if (string.IsNullOrWhiteSpace(studentIdOrAsurite))
                            return UnprocessableEntity($"Missing 'Student_ID (ID number OR ASUrite accepted)' in row {rowNum}");
                        if (string.IsNullOrWhiteSpace(classNum))
                            return UnprocessableEntity($"Missing 'ClassNum' in row {rowNum}");

                        // Default FultonFellow to "No" if empty
                        if (string.IsNullOrWhiteSpace(fultonFellow))
                            fultonFellow = "No";

                        int weeklyHours = int.Parse(weeklyHoursStr);

                        // Lookup student
                        YourNamespace.Models.StudentLookup student = null;
                        if (int.TryParse(studentIdOrAsurite, out int studentId))
                        {
                            student = await _context.StudentLookups.FirstOrDefaultAsync(s => s.Student_ID == studentId);
                        }
                        else
                        {
                            student = await _context.StudentLookups.FirstOrDefaultAsync(s => s.ASUrite.ToLower() == studentIdOrAsurite.ToLower());
                        }

                        if (student == null)
                            return UnprocessableEntity($"Student '{studentIdOrAsurite}' not found (row {rowNum})");

                        // Lookup class
                        var classObj = await _context.ClassSchedule2254.FirstOrDefaultAsync(c => c.ClassNum == classNum);
                        if (classObj == null)
                            return UnprocessableEntity($"ClassNum '{classNum}' not found (row {rowNum})");

                        // Create assignment
                        var assignment = new StudentClassAssignment
                        {
                            Student_ID = student.Student_ID,
                            ASUrite = student.ASUrite,
                            Position = position,
                            FultonFellow = fultonFellow,
                            WeeklyHours = weeklyHours,
                            Email = student.ASU_Email_Adress,
                            First_Name = student.First_Name,
                            Last_Name = student.Last_Name,
                            EducationLevel = student.Degree,
                            Subject = classObj.Subject,
                            CatalogNum = classObj.CatalogNum,
                            ClassSession = classObj.Session,
                            ClassNum = classNum,
                            Term = classObj.Term,
                            InstructorFirstName = classObj.InstructorFirstName,
                            InstructorLastName = classObj.InstructorLastName,
                            InstructorID = classObj.InstructorID,
                            Location = classObj.Location,
                            Campus = classObj.Campus,
                            AcadCareer = classObj.AcadCareer,
                            CreatedAt = now,
                            cum_gpa = student.Cumulative_GPA,
                            cur_gpa = student.Current_GPA
                        };

                        assignment.Compensation = AssignmentUtils.CalculateCompensation(assignment);
                        assignment.CostCenterKey = AssignmentUtils.ComputeCostCenterKey(assignment);

                        assignments.Add(assignment);
                    }
                    else
                    {
                        // OLD FORMAT: 17 fields - all data provided in CSV
                        var record = csv.GetRecord<StudentClassAssignment>();

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
                        }

                        assignments.Add(record);
                    }
                }
            }

            _context.StudentClassAssignments.AddRange(assignments);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"{assignments.Count} records uploaded successfully." });
        }

        // POST /calibrate-preview - Preview CSV data before saving
        [HttpPost("calibrate-preview")]
        public async Task<IActionResult> CalibratePreview([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty or missing.");

            if (!file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                return BadRequest("CSV file required.");

            var previewData = new List<object>();

            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                TrimOptions = TrimOptions.Trim,
                IgnoreBlankLines = true,
                HeaderValidated = null,
                MissingFieldFound = null
            }))
            {
                csv.Read();
                csv.ReadHeader();
                var headers = csv.HeaderRecord;

                // Only process new 5-field format for preview
                if (!headers.Any(h => h.Contains("Student_ID (ID number OR ASUrite accepted)")))
                    return BadRequest("Preview only supports the new 5-field template format.");

                int rowNum = 1;
                while (csv.Read())
                {
                    rowNum++;

                    var position = csv.GetField<string>("Position")?.Trim();
                    var fultonFellow = csv.GetField<string>("FultonFellow")?.Trim();
                    var weeklyHoursStr = csv.GetField<string>("WeeklyHours")?.Trim();
                    var studentIdOrAsurite = csv.GetField<string>("Student_ID (ID number OR ASUrite accepted)")?.Trim();
                    var classNum = csv.GetField<string>("ClassNum")?.Trim();

                    // Validate required fields
                    if (string.IsNullOrWhiteSpace(position) || string.IsNullOrWhiteSpace(weeklyHoursStr) ||
                        string.IsNullOrWhiteSpace(studentIdOrAsurite) || string.IsNullOrWhiteSpace(classNum))
                        return UnprocessableEntity($"Missing required field in row {rowNum}");

                    // Default FultonFellow
                    if (string.IsNullOrWhiteSpace(fultonFellow))
                        fultonFellow = "No";

                    // Lookup student
                    StudentLookup student = null;
                    if (int.TryParse(studentIdOrAsurite, out int studentId))
                    {
                        student = await _context.StudentLookups.FirstOrDefaultAsync(s => s.Student_ID == studentId);
                    }
                    else
                    {
                        student = await _context.StudentLookups.FirstOrDefaultAsync(s => s.ASUrite.ToLower() == studentIdOrAsurite.ToLower());
                    }

                    if (student == null)
                        return UnprocessableEntity($"Student not found for '{studentIdOrAsurite}' (row {rowNum})");

                    // Lookup class
                    var classObj = await _context.ClassSchedule2254.FirstOrDefaultAsync(c => c.ClassNum == classNum);
                    if (classObj == null)
                        return UnprocessableEntity($"ClassNum not found: '{classNum}' (row {rowNum})");

                    // Build preview object
                    previewData.Add(new
                    {
                        Position = position,
                        FultonFellow = fultonFellow,
                        WeeklyHours = weeklyHoursStr,
                        Student_ID = student.Student_ID,
                        ASUrite = student.ASUrite,
                        ClassNum = classObj.ClassNum,
                        First_Name = student.First_Name,
                        Last_Name = student.Last_Name,
                        ASU_Email_Adress = student.ASU_Email_Adress,
                        Degree = student.Degree,
                        cum_gpa = student.Cumulative_GPA,
                        cur_gpa = student.Current_GPA,
                        Subject = classObj.Subject,
                        CatalogNum = classObj.CatalogNum,
                        SectionNum = classObj.SectionNum,
                        Title = classObj.Title,
                        Term = classObj.Term,
                        Session = classObj.Session,
                        InstructorID = classObj.InstructorID,
                        InstructorFirstName = classObj.InstructorFirstName,
                        InstructorLastName = classObj.InstructorLastName,
                        InstructorEmail = classObj.InstructorEmail,
                        Location = classObj.Location,
                        Campus = classObj.Campus,
                        AcadCareer = classObj.AcadCareer
                    });
                }
            }

            return Ok(previewData);
        }

        // GET /student-summary/{identifier} - Get student assignment summary
        [HttpGet("student-summary/{identifier}")]
        public async Task<IActionResult> GetAssignmentSummary(string identifier)
        {
            // Lookup student by Student_ID or ASUrite
            StudentLookup student = null;
            if (int.TryParse(identifier, out int studentId))
            {
                student = await _context.StudentLookups.FirstOrDefaultAsync(s => s.Student_ID == studentId);
            }
            else
            {
                student = await _context.StudentLookups.FirstOrDefaultAsync(s => s.ASUrite.ToLower() == identifier.ToLower());
            }

            if (student == null)
                return NotFound("Student not found");

            // Get all assignments for this student (excluding instructor-edited ones)
            var assignments = await _context.StudentClassAssignments
                .Where(a => a.Student_ID == student.Student_ID && a.Instructor_Edit == null)
                .ToListAsync();

            // Tally hours by session
            var sessionHours = new Dictionary<string, int> { { "A", 0 }, { "B", 0 }, { "C", 0 } };
            var assignmentList = new List<object>();

            foreach (var a in assignments)
            {
                var session = (a.ClassSession ?? "").Trim().ToUpper();
                if (sessionHours.ContainsKey(session))
                    sessionHours[session] += a.WeeklyHours ?? 0;

                assignmentList.Add(new
                {
                    Id = a.Id,
                    Position = a.Position,
                    WeeklyHours = a.WeeklyHours,
                    ClassSession = a.ClassSession,
                    Subject = a.Subject,
                    CatalogNum = a.CatalogNum,
                    ClassNum = a.ClassNum,
                    InstructorName = $"{a.InstructorFirstName} {a.InstructorLastName}",
                    AcadCareer = a.AcadCareer
                });
            }

            // Compose response
            return Ok(new
            {
                StudentName = $"{student.First_Name ?? ""} {student.Last_Name ?? ""}".Trim(),
                ASUrite = student.ASUrite,
                Student_ID = student.Student_ID,
                Position = assignments.FirstOrDefault()?.Position,
                FultonFellow = assignments.FirstOrDefault()?.FultonFellow,
                EducationLevel = assignments.FirstOrDefault()?.EducationLevel,
                sessionA = sessionHours["A"],
                sessionB = sessionHours["B"],
                sessionC = sessionHours["C"],
                assignments = assignmentList
            });
        }

        // POST /bulk-edit - Bulk edit assignments with Instructor_Edit tracking
        [HttpPost("bulk-edit")]
        public async Task<IActionResult> BulkEditAssignments([FromBody] BulkEditRequest request)
        {
            var studentId = request.StudentId;
            var updateRows = request.Updates ?? new List<AssignmentEditDto>();
            var deleteIds = request.Deletes ?? new List<int>();

            // Find student
            if (string.IsNullOrWhiteSpace(studentId))
                return BadRequest("No studentId provided");

            StudentLookup student = null;
            if (int.TryParse(studentId, out int sid))
            {
                student = await _context.StudentLookups.FirstOrDefaultAsync(s => s.Student_ID == sid);
            }
            else
            {
                student = await _context.StudentLookups.FirstOrDefaultAsync(s => s.ASUrite.ToLower() == studentId.ToLower());
            }

            if (student == null)
                return NotFound("Student not found");

            var updatedResponse = new List<object>();
            var editableFields = new[] { "Position", "WeeklyHours", "ClassNum" };

            // 1. Handle updates/edits
            foreach (var edit in updateRows)
            {
                var orig = await _context.StudentClassAssignments.FirstOrDefaultAsync(a => a.Id == edit.Id);
                if (orig == null) continue;

                // Mark original as edited
                orig.Instructor_Edit = "Y";
                _context.StudentClassAssignments.Update(orig);
                await _context.SaveChangesAsync();

                ClassSchedule2254 classObj = null;
                if (!string.IsNullOrWhiteSpace(edit.ClassNum) && edit.ClassNum != orig.ClassNum)
                {
                    classObj = await _context.ClassSchedule2254.FirstOrDefaultAsync(c => c.ClassNum == edit.ClassNum && c.Term == orig.Term);
                    if (classObj == null)
                        return NotFound($"ClassNum {edit.ClassNum} not found");
                }

                // Create new assignment
                var newAssign = new StudentClassAssignment
                {
                    Student_ID = orig.Student_ID,
                    ASUrite = orig.ASUrite,
                    Position = edit.Position ?? orig.Position,
                    WeeklyHours = edit.WeeklyHours ?? orig.WeeklyHours,
                    FultonFellow = orig.FultonFellow,
                    Email = orig.Email,
                    EducationLevel = orig.EducationLevel,
                    Subject = classObj?.Subject ?? orig.Subject,
                    CatalogNum = classObj?.CatalogNum ?? orig.CatalogNum,
                    ClassSession = classObj?.Session ?? orig.ClassSession,
                    ClassNum = edit.ClassNum ?? orig.ClassNum,
                    Term = orig.Term,
                    InstructorFirstName = classObj?.InstructorFirstName ?? orig.InstructorFirstName,
                    InstructorLastName = classObj?.InstructorLastName ?? orig.InstructorLastName,
                    InstructorID = classObj?.InstructorID ?? orig.InstructorID,
                    Location = classObj?.Location ?? orig.Location,
                    Campus = classObj?.Campus ?? orig.Campus,
                    AcadCareer = classObj?.AcadCareer ?? orig.AcadCareer,
                    cur_gpa = orig.cur_gpa,
                    cum_gpa = orig.cum_gpa,
                    CreatedAt = DateTime.UtcNow,
                    Instructor_Edit = null,
                    First_Name = orig.First_Name,
                    Last_Name = orig.Last_Name,
                    Position_Number = orig.Position_Number,
                    SSN_Sent = orig.SSN_Sent,
                    Offer_Sent = orig.Offer_Sent,
                    Offer_Signed = orig.Offer_Signed
                };

                // Recalculate compensation and cost center
                newAssign.Compensation = AssignmentUtils.CalculateCompensation(newAssign);
                newAssign.CostCenterKey = AssignmentUtils.ComputeCostCenterKey(newAssign);

                _context.StudentClassAssignments.Add(newAssign);
                await _context.SaveChangesAsync();

                // Determine changed fields
                var changedFields = new List<string>();
                if (edit.Position != null && edit.Position != orig.Position) changedFields.Add("Position");
                if (edit.WeeklyHours != null && edit.WeeklyHours != orig.WeeklyHours) changedFields.Add("WeeklyHours");
                if (edit.ClassNum != null && edit.ClassNum != orig.ClassNum) changedFields.Add("ClassNum");

                updatedResponse.Add(new
                {
                    Id = newAssign.Id,
                    Student_ID = newAssign.Student_ID,
                    Position = newAssign.Position,
                    WeeklyHours = newAssign.WeeklyHours,
                    ClassNum = newAssign.ClassNum,
                    Subject = newAssign.Subject,
                    CatalogNum = newAssign.CatalogNum,
                    ClassSession = newAssign.ClassSession,
                    Compensation = newAssign.Compensation,
                    CostCenterKey = newAssign.CostCenterKey,
                    changed_fields = changedFields
                });
            }

            // 2. Handle deletions
            foreach (var delId in deleteIds)
            {
                var orig = await _context.StudentClassAssignments.FirstOrDefaultAsync(a => a.Id == delId);
                if (orig != null)
                {
                    orig.Instructor_Edit = "D";
                    _context.StudentClassAssignments.Update(orig);
                }
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                updated = updatedResponse,
                deleted = deleteIds,
                status = "success"
            });
        }

         [HttpGet("template")]
         public IActionResult DownloadTemplate()
            {
              // New simplified template with only 5 fields
              var headers = new[]
                {
                    "Position",
                    "FultonFellow",
                    "WeeklyHours",
                    "Student_ID (ID number OR ASUrite accepted)",
                    "ClassNum"
                };

                var csv = new StringBuilder();
                csv.AppendLine(string.Join(",", headers));

                var bytes = Encoding.UTF8.GetBytes(csv.ToString());
                return File(bytes, "text/csv", "BulkUploadTemplate.csv");
         }
    }
}
