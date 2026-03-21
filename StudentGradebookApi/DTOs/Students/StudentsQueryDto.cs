using StudentGradebookApi.DTOs.SharedDto;

namespace StudentGradebookApi.DTOs.Students
{
    public class StudentsQueryDto : QueryDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTimeOffset? DateOfBirth { get; set; }
        public DateTimeOffset? EnrollmentDate { get; set; }
        public string? Status { get; set; }
    }
}
