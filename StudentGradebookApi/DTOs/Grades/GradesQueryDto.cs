using StudentGradebookApi.DTOs.SharedDto;
using System.Text.Json.Serialization;

namespace StudentGradebookApi.DTOs.Grades
{
    public class GradesQueryDto : QueryDto
    {
        [JsonPropertyName("firstName")]
        public string? FirstName { get; set; }
        [JsonPropertyName("lastName")]
        public string? LastName { get; set; }
        [JsonPropertyName("classSubjectId")]
        public int ClassSubjectId { get; set; } = 1;
        [JsonPropertyName("gradingYear")]
        public int GradingYear { get; set; } = DateTime.Now.Year;
        [JsonPropertyName("gradingMonth")]
        public int GradingMonth { get; set; } = DateTime.Now.Month;
    }
}
