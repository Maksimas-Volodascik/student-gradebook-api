using Microsoft.AspNetCore.Mvc;
using StudentGradebookApi.DTOs.SharedDto;

namespace StudentGradebookApi.DTOs.Teachers
{
    public class TeachersQueryDto : QueryDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? Room {  get; set; }
        public string? AcademicYear { get; set; }
        public string? SubjectName { get; set; }
        public string? SubjectCode { get; set; }
    }
}
