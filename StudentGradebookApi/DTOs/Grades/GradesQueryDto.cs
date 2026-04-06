using StudentGradebookApi.DTOs.SharedDto;
using System.Text.Json.Serialization;

namespace StudentGradebookApi.DTOs.Grades
{
    public class GradesQueryDto : QueryDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int ClassSubjectId { get; set; } = 1;
        public int GradingYear { get; set; } = DateTime.Now.Year;
        public int GradingMonth { get; set; } = DateTime.Now.Month;
    }
}
