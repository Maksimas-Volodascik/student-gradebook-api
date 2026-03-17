using StudentGradebookApi.DTOs.Grades;
using StudentGradebookApi.DTOs.SubjectClass;

namespace StudentGradebookApi.DTOs.Teachers
{
    public class TeacherDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;   
        public string LastName { get; set; } = string.Empty;
        public List<ClassSubjectDTO> ClassSubjects { get; set; } = new List<ClassSubjectDTO>();
    }
}
