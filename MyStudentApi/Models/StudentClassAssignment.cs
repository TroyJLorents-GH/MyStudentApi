using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyStudentApi.Models
{
    [Table("StudentClassAssignments", Schema = "dbo")]
    public class StudentClassAssignment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Student_ID { get; set; }
        public string Position { get; set; }
       public int WeeklyHours { get; set; }
        public string FultonFellow { get; set; }
        public string Email { get; set; }
        public string EducationLevel { get; set; }
        public string Subject { get; set; }
        public int CatalogNum { get; set; }
        public string ClassSession { get; set; }
        public string ClassNum { get; set; }
        public string Term { get; set; }
       // public string InstructorName { get; set; }
        public string InstructorFirstName { get; set; }
        public string InstructorLastName { get; set; }
        public double Compensation { get; set; }
        public DateTime? CreatedAt { get; set; }

        [MaxLength(50)]
        public string Location { get; set; }
        public string Campus { get; set; }
        public string AcadCareer { get; set; }
        public string CostCenterKey { get; set; }
        public string? First_Name { get; set; }
        public string? Last_Name { get; set; }
        public string? Position_Number { get; set; }
        public bool? I9_Sent { get; set; } // allows true/false/null
        public bool? SSN_Sent { get; set; }
        public bool? Offer_Sent { get; set; }
        public bool? Offer_Signed { get; set; }
        public double? cur_gpa { get; set; }
        public double? cum_gpa { get; set; }

    }
}
