using StudentGradebookApi.DTOs.Subjects;

namespace StudentGradebookApi.DTOs.Students
{
    public class StudentEnrolledSubject
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public DateTimeOffset EnrollmentDate { get; set; }
        public string Status { get; set; }
        public List<string> Subjects { get; set; } = new List<string> {""};

    }
}
