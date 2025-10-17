namespace MyStudentApi.DTO
{
    public class BulkEditRequest
    {
        public string StudentId { get; set; }
        public List<AssignmentEditDto> Updates { get; set; }
        public List<int> Deletes { get; set; }
    }

    public class AssignmentEditDto
    {
        public int Id { get; set; }
        public string? Position { get; set; }
        public int? WeeklyHours { get; set; }
        public string? ClassNum { get; set; }
    }
}
