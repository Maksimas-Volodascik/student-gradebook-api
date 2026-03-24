using StudentGradebookApi.DTOs.Grades;
using StudentGradebookApi.DTOs.SubjectClass;

namespace StudentGradebookApi.DTOs.Teachers
{
    public class TeacherDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;   
        public string LastName { get; set; } = string.Empty;
        public List<ClassSubjectDto> ClassSubjects { get; set; } = new List<ClassSubjectDto>();
    }
}
