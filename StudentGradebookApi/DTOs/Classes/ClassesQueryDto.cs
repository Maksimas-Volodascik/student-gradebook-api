using StudentGradebookApi.DTOs.SharedDto;

namespace StudentGradebookApi.DTOs.Classes
{
    public class ClassesQueryDto : QueryDto
    {
        public string? StartingYear { get; set; }
        public int? Room { get; set; }
        public string? SubjectName { get; set; }
    }
}
