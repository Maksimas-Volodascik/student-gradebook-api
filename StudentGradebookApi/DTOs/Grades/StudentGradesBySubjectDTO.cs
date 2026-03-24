namespace StudentGradebookApi.DTOs.Grades
{
    public class StudentGradesBySubjectDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ClassSubjectId { get; set; }
        public int EnrollmentId { get; set; }
        public List<GradesListDto> Grades { get; set; } = new List<GradesListDto>();
    }
}
