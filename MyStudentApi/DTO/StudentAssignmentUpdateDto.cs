namespace MyStudentApi.DTO
{
    public class StudentAssignmentUpdateDto
    {
        public string? Position_Number { get; set; }
        public bool? I9_Sent { get; set; }
        public bool? SSN_Sent { get; set; }
        public bool? Offer_Sent { get; set; }
        public bool? Offer_Signed { get; set; }
    }
}
