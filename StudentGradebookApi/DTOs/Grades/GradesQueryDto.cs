using StudentGradebookApi.DTOs.SharedDto;

namespace StudentGradebookApi.DTOs.Grades
{
    public class GradesQueryDto : QueryDto
    {
        //Filters
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int ClassSubjectId { get; set; } = 1;
        public int GradingYear { get; set; } = DateTime.Now.Year;
        public int GradingMonth { get; set; } = DateTime.Now.Month;
    }
}
