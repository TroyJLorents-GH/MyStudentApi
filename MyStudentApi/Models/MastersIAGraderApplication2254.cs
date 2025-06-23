using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyStudentApi.Models
{
    [Table("MastersIAGraderApplication2254", Schema = "dbo")]
    public class MastersIAGraderApplication2254
    {
        [Key]
        public int Id { get; set; }

        public DateTime? Start_time { get; set; }
        public DateTime? Completion_time { get; set; }

        [Required, StringLength(50)]
        public string Email { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }

        [Required, Column("Your_ASU_Email_address"), StringLength(50)]
        public string YourASUEmailAddress { get; set; }

        [Required, Column("First_Name"), StringLength(50)]
        public string FirstName { get; set; }

        [Required, Column("Last_Name"), StringLength(50)]
        public string LastName { get; set; }

        [Required, Column("ASU_10_digit_ID")]
        public int ASU10DigitID { get; set; }

        [Required, Column("Degree_Program_you_are_enrolled_in"), StringLength(100)]
        public string DegreeProgram { get; set; }

        [Column("For_M_S_students_choose_culminating_experience_option"), StringLength(500)]
        public string? CulminatingExperienceOption { get; set; }

        [Column("Faculty_Thesis_Dissertation_Advisor_if_applicable_for_thesis"), StringLength(100)]
        public string? FacultyAdvisor { get; set; }

        [Column("TA_Speak_Test_Score_or_iBT"), StringLength(100)]
        public string? TASpeakTestScoreOrIBT { get; set; }

        [Column("Experience_Summary")]
        public string? ExperienceSummary { get; set; } // nvarchar(max)

        [Required, Column("What_programming_languages_are_you_familiar_with")]
        public string ProgrammingLanguages { get; set; } // nvarchar(max)

        [Column("If_you_did_not_pass_the_TA_Speak_Test_are_you_attending"), StringLength(50)]
        public string? SpeakTestRemediation { get; set; }

        [Column("For_MS_Thesis_students_Did_you_complete_your_Dissertation_Thesis_Proposal"), StringLength(50)]
        public string? DissertationProposalStatus { get; set; }

        [Required, Column("Your_undergraduate_Institution_University_of_your_bachelors_degree"), StringLength(500)]
        public string UndergraduateInstitution { get; set; }

        [Required, Column("Cumulative_undergraduate_GPA"), StringLength(250)]
        public string UndergraduateGPA { get; set; }

        [Required, Column("First_Semester_enrolled_grad_program"), StringLength(350)]
        public string FirstSemesterGrad { get; set; }

        [Required, Column("GPA_of_graduate_courses_completed_so_far_at_ASU_type_NONE_if_no_GPA_earned_yet_at_ASU"), StringLength(500)]
        public string GraduateGPA { get; set; }

        [Required]
        public DateTime Expected_graduation_semester_month_year_in_mm_yyyy_format { get; set; }

        [Column("Which_positions_would_you_like_to_be_considered_for_Select_all_that_apply")]
        public string? PositionsConsidered { get; set; } // nvarchar(max)

        [Column("How_many_hours_per_week_would_you_be_available_to_work"), StringLength(250)]
        public string? HoursAvailable { get; set; }

        [Column("Provide_preferred_courses_that_you_would_like_to_be_be_considered_for_if_any"), StringLength(2500)]
        public string? PreferredCourses { get; set; }

        [Column("Please_upload_your_ASU_transcript_in_pdf_format")]
        public string? TranscriptUrl { get; set; } // nvarchar(max)

        [Column("Provide_a_web_link_pointing_to_a_copy_of_your_Resume_E_g_LinkedIn_or_cloud_location")]
        public string? ResumeUrl { get; set; } // nvarchar(max)

        [Required, Column("I_agree"), StringLength(50)]
        public string IAgree { get; set; }
    }
}







//using System;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace MyStudentApi.Models
//{
//    [Table("MastersIAGraderApplication2254", Schema = "dbo")]
//    public class MastersIAGraderApplication2254
//    {
//        [Key]
//        public int Id { get; set; }

//        [Required, StringLength(50)]
//        public string? Email { get; set; }

//        [Required, StringLength(50)]
//        public string? Name { get; set; }

//        [Required, Column("Your_ASU_Email_address"), StringLength(50)]
//        public string? YourASUEmailAddress { get; set; }

//        [Required, Column("First_Name"), StringLength(50)]
//        public string? FirstName { get; set; }

//        [Required, Column("Last_Name"), StringLength(50)]
//        public string? LastName { get; set; }

//        [Required, Column("ASU_10_digit_ID")]
//        public int ASU10DigitID { get; set; }

//        [Required, Column("Degree_Program_you_are_enrolled_in"), StringLength(100)]
//        public string? DegreeProgram { get; set; }

//        [Column("For_M_S_students_choose_culminating_experience_option"), StringLength(50)]
//        public string? CulminatingExperienceOption { get; set; }

//        [Required, StringLength(500)]
//        public string? What_programming_languages_are_you_familiar_with { get; set; }

//        [Required, Column("Cumulative_undergraduate_GPA"), StringLength(50)]
//        public string? UndergraduateGPA { get; set; }

//        [Required, Column("GPA_of_graduate_courses_completed_so_far_at_ASU_type_NONE_if_no_GPA_earned_yet_at_ASU"), StringLength(50)]
//        public string? GraduateGPA { get; set; }

//        [Required]
//        public DateTime Expected_graduation_semester_month_year_in_mm_yyyy_format { get; set; }

//        [Required, Column("Which_positions_would_you_like_to_be_considered_for_Select_all_that_apply")]
//        public string? PositionsConsidered { get; set; } // nvarchar(MAX)

//        [Column("How_many_hours_per_week_would_you_be_available_to_work"), StringLength(100)]
//        public string? HoursAvailable { get; set; }

//        [Column("Provide_preferred_courses_that_you_would_like_to_be_be_considered_for_if_any"), StringLength(550)]
//        public string? PreferredCourses { get; set; }

//        [Column("Please_upload_your_ASU_transcript_in_pdf_format")]
//        public string? TranscriptUrl { get; set; } // nvarchar(MAX)

//        [Column("Provide_a_web_link_pointing_to_a_copy_of_your_Resume_E_g_LinkedIn_or_cloud_location")]
//        public string? ResumeUrl { get; set; } // nvarchar(MAX)

//        [Required, Column("First_Semester_enrolled_grad_program"), StringLength(50)]
//        public string? FirstSemesterGrad { get; set; }

//        //public DateTime? Start_time { get; set; }
//        //public DateTime? Completion_time { get; set; }



//        [Column("Faculty_Thesis_Dissertation_Advisor_if_applicable_for_thesis"), StringLength(50)]
//        public string? FacultyAdvisor { get; set; }

//        [Column("TA_Speak_Test_Score_or_iBT"), StringLength(100)]
//        public string? TASpeakTestScoreOrIBT { get; set; }

//        [Column("If_you_did_not_pass_the_TA_Speak_Test_are_you_attending"), StringLength(50)]
//        public string? NotPassingTASpeak_Test_Attending { get; set; }

//        [Column("For_MS_Thesis_students_Did_you_complete_your_Dissertation_Thesis_Proposal"), StringLength(50)]
//        public string? DissertationProposalStatus { get; set; }

//        [Required, Column("Your_undergraduate_Institution_University_of_your_bachelors_degree"), StringLength(100)]
//        public string? UndergraduateInstitution { get; set; }

//        //[Required, Column("I_agree"), StringLength(50)]
//        //public string IAgree { get; set; }
//    }
//}
