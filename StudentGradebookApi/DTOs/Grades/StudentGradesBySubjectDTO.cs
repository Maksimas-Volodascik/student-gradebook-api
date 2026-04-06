using System.Text.Json.Serialization;

namespace StudentGradebookApi.DTOs.Grades
{
    public class StudentGradesBySubjectDto
    {
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }
        [JsonPropertyName("lastName")]
        public string LastName { get; set; }
        [JsonPropertyName("classSubjectId")]
        public int ClassSubjectId { get; set; }
        [JsonPropertyName("enrollmentId")]
        public int EnrollmentId { get; set; }
        [JsonPropertyName("grades")]
        public List<GradesListDto> Grades { get; set; } = new List<GradesListDto>();
    }
}
