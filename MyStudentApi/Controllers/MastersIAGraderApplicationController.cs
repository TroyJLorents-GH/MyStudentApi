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
    public class MastersIAGraderApplicationController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MastersIAGraderApplicationController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/MastersIAGraderApplication
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationSummaryDto>>> GetApplicationSummaries()
        {
            var summaries = await _context.MastersIAGraderApplications
                .Select(app => new ApplicationSummaryDto
                {
                    Id = app.Id,
                    Email = app.Email,
                    Name = app.Name,
                    YourASUEmailAddress = app.YourASUEmailAddress,
                    FirstName = app.FirstName,
                    LastName = app.LastName,
                    ASU10DigitID = app.ASU10DigitID,
                    DegreeProgram = app.DegreeProgram,
                    GraduateGPA = app.GraduateGPA,
                    PositionsConsidered = app.PositionsConsidered,
                    PreferredCourses = app.PreferredCourses,
                    HoursAvailable = app.HoursAvailable,
                    TranscriptUrl = app.TranscriptUrl,
                    ProgrammingLanguage = app.ProgrammingLanguages,
                    ResumeUrl = app.ResumeUrl,
                    ExpectedGraduation = app.Expected_graduation_semester_month_year_in_mm_yyyy_format,
                     DissertationProposalStatus = app.DissertationProposalStatus,
                    UndergraduateInstitution = app.UndergraduateInstitution,
                    UndergraduateGPA = app.UndergraduateGPA
                })
                .ToListAsync();

            return summaries;
        }
    }
}
