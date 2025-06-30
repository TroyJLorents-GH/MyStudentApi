using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyStudentApi.Data;
using MyStudentApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyStudentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClassController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Class/subjects?term=2254
        [HttpGet("subjects")]
        public async Task<ActionResult<IEnumerable<string>>> GetSubjects([FromQuery] string term)
        {
            if (string.IsNullOrEmpty(term))
                return BadRequest("Term is required.");

            // For this example, we focus on term "2254"
            if (term == "2254")
            {
                var subjects = await _context.ClassSchedule2254
                    .Where(c => c.Term == term)
                    .Select(c => c.Subject)
                    .Distinct()
                    .ToListAsync();
                return subjects;
            }
            else if (term == "2251")
            {
                var subjects = await _context.ClassLookups
                    .Where(c => c.Term == term)
                    .Select(c => c.Subject)
                    .Distinct()
                    .ToListAsync();
                return subjects;
            }
            else
            {
                return BadRequest("Invalid term value.");
            }
        }

        // GET: api/Class/catalog?term=2254&subject=XYZ
        [HttpGet("catalog")]
        public async Task<ActionResult<IEnumerable<string>>> GetCatalogNumbers([FromQuery] string term, [FromQuery] string subject)
        {
            if (string.IsNullOrEmpty(term) || string.IsNullOrEmpty(subject))
                return BadRequest("Term and Subject are required.");

            if (term == "2254")
            {
                var catalogs = await _context.ClassSchedule2254
                    .Where(c => c.Term == term && c.Subject == subject)
                    .Select(c => c.CatalogNum.ToString())
                    .Distinct()
                    .ToListAsync();
                return catalogs;
            }
            else if (term == "2251")
            {
                var catalogs = await _context.ClassLookups
                    .Where(c => c.Term == term && c.Subject == subject)
                    .Select(c => c.CatalogNum.ToString())
                    .Distinct()
                    .ToListAsync();
                return catalogs;
            }
            else
            {
                return BadRequest("Invalid term value.");
            }
        }

        // GET: api/Class/classnumbers?term=2254&subject=XYZ&catalogNum=123
        [HttpGet("classnumbers")]
        public async Task<ActionResult<IEnumerable<string>>> GetClassNumbers([FromQuery] string term, [FromQuery] string subject, [FromQuery] string catalogNum)
        {
            if (string.IsNullOrEmpty(term) || string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(catalogNum))
                return BadRequest("Term, Subject, and CatalogNum are required.");

            if (!short.TryParse(catalogNum, out short catNum))
                return BadRequest("CatalogNum must be numeric.");

            if (term == "2254")
            {
                var classNumbers = await _context.ClassSchedule2254
                    .Where(c => c.Term == term && c.Subject == subject && c.CatalogNum == catNum)
                    .Select(c => c.ClassNum)
                    .Distinct()
                    .ToListAsync();
                return classNumbers;
            }
            else if (term == "2251")
            {
                var classNumbers = await _context.ClassLookups
                    .Where(c => c.Term == term && c.Subject == subject && c.CatalogNum == catNum)
                    .Select(c => c.ClassNum)
                    .Distinct()
                    .ToListAsync();
                return classNumbers;
            }
            else
            {
                return BadRequest("Invalid term value.");
            }
        }

        // GET: api/Class/details/{classNum}?term=2254
        [HttpGet("details/{classNum}")]
        public async Task<ActionResult<object>> GetClassDetails(string classNum, [FromQuery] string term)
        {
            if (string.IsNullOrEmpty(classNum) || string.IsNullOrEmpty(term))
                return BadRequest("ClassNum and Term are required.");

            if (term == "2254")
            {
                var classInfo = await _context.ClassSchedule2254.FirstOrDefaultAsync(c => c.ClassNum == classNum && c.Term == term);
                if (classInfo == null)
                    return NotFound();
                return new
                {
                    classInfo.Session,
                    classInfo.Term,
                    classInfo.InstructorID,
                    classInfo.InstructorFirstName,
                    classInfo.InstructorLastName,
                    classInfo.InstructorEmail,
                    classInfo.Location,
                    classInfo.Campus,
                    classInfo.AcadCareer
                };
            }
            else if (term == "2251")
            {
                var classInfo = await _context.ClassLookups.FirstOrDefaultAsync(c => c.ClassNum == classNum && c.Term == term);
                if (classInfo == null)
                    return NotFound();
                return new
                {
                    classInfo.Session,
                    classInfo.Term,
                    classInfo.InstructorID,
                    classInfo.InstructorFirstName,
                    classInfo.InstructorLastName,
                    classInfo.InstructorEmail,
                    classInfo.Location
                };
            }
            else
            {
                return BadRequest("Invalid term value.");
            }
        }
    }
}
