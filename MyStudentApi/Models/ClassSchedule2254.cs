using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyStudentApi.Models
{
    [Table("ClassSchedule2254", Schema = "dbo")]
    public class ClassSchedule2254
    {
        [Key]
        public string ClassNum { get; set; }  // As a string

        public string? Term { get; set; }
        public string Session { get; set; }
        public string Subject { get; set; }
        public int CatalogNum { get; set; } // smallint
        public int SectionNum { get; set; } // smallint
        public string Title { get; set; }
        public int? InstructorID { get; set; }
        public string InstructorLastName { get; set; }
        public string InstructorFirstName { get; set; }
        public string InstructorEmail { get; set; }
        public string Location { get; set; }
        public string Campus { get; set; }
        public string AcadCareer { get; set; }
    }
}
