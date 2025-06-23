using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyStudentApi.Models
{
    [Table("ClassLookups", Schema = "dbo")]
    public class ClassLookup
    {
        [Key]
        public string ClassNum { get; set; }

        public string? Term { get; set; }

        [MaxLength(50)]
        public string Session { get; set; }

        //[MaxLength(50)]
        //public string Location { get; set; }

        //[MaxLength(50)]
        //public string AcadCareer { get; set; }

        [MaxLength(50)]
        public string Subject { get; set; }

        public int CatalogNum { get; set; }

        public int SectionNum { get; set; }

        [MaxLength(50)]
        public string Component { get; set; }

        // Title – if you want a fixed length, you can add a MaxLength attribute; otherwise, this can be unbounded.
        public string Title { get; set; }

        // For SQL TIME types, use TimeSpan? in C#
        //public TimeSpan? StartTime { get; set; }
        //public TimeSpan? EndTime { get; set; }

        //[MaxLength(25)]
        //public string DaysMet { get; set; }

        //[MaxLength(50)]
        //public string FacilityID { get; set; }

        //public DateTime? StartDate { get; set; }
        //public DateTime? EndDate { get; set; }

        public int InstructorID { get; set; }

        [MaxLength(100)]
        public string InstructorLastName { get; set; }

        [MaxLength(100)]
        public string InstructorFirstName { get; set; }

        [MaxLength(100)]
        public string InstructorEmail { get; set; }

        [MaxLength(50)]
        public string Location { get; set; }

    }
}
