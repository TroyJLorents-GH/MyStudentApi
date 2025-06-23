using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyStudentApi.Models
{
    public class ApplicationSummaryDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string YourASUEmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ASU10DigitID { get; set; }
        public string DegreeProgram { get; set; }
        public DateTime ExpectedGraduation { get; set; }
        public string GraduateGPA { get; set; }
        public string PositionsConsidered { get; set; }
        public string PreferredCourses { get; set; }
        public string TranscriptUrl { get; set; } // nvarchar(MAX)
        public string ResumeUrl { get; set; } // nvarchar(MAX)
        public string HoursAvailable { get; set; }
        public string? ProgrammingLanguage { get; set; }
        public string? TASpeakTestScoreOrIBT { get; set; }

        public string? DissertationProposalStatus { get; set; }

        [Required, StringLength(500)]
        public string UndergraduateInstitution { get; set; }

        [Required, StringLength(250)]
        public string UndergraduateGPA { get; set; }
    }
}
