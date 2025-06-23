using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyStudentApi.Models
{
    // Map this entity to the SPR25_Hiring table in the dbo schema.
    [Table("SPR25_Hiring", Schema = "dbo")]
    public class Student
    {
        // This property maps to the PK column, which is the primary key.
        [Key]
        public string? PK { get; set; }

        public string Position { get; set; }

        //public int Hours { get; set; }

        public int Student_ID { get; set; }

        public string? First_Name { get; set; }

        public string? Last_Name { get; set; }

        public string? Email { get; set; }

        public string? Education_Level { get; set; }

        //public string Course_Prefix_1 { get; set; }

        //public string Course_1_Number { get; set; }

        //public string Course_1_Instructor { get; set; }

        //public string Session_Component { get; set; }

        public string? CC_Account { get; set; }

        // Assuming Compensation is stored as a decimal
        public double Compensation { get; set; }

        public string? Position_Number { get; set; }
    }
}
