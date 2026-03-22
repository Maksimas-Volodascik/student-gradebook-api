namespace StudentGradebookApi.DTOs.SubjectClass
{
    public class ClassSubjectDto
    {
        public string AcademicYear { get; set; } = null!;
        public int Room { get; set; }
        public string SubjectName { get; set; } = null!;
        public string SubjectCode { get; set; } = null!;
    }
}
