using StudentGradebookApi.DTOs.Grades;
using StudentGradebookApi.Models;
using StudentGradebookApi.Shared;

namespace StudentGradebookApi.Services.GradesServices
{
    public interface IGradesServices
    {
        Task<Result<IEnumerable<StudentGradesBySubjectDTO>>> GetStudentGradesBySubjectId(GradesQueryDto queryDto);
        Task<Result<IEnumerable<StudentGradesBySubjectDTO>>> GetStudentGradesByStudentId(int year, int month);
        Task<Result> AddGradeAsync(NewGradeDTO newGrade);
        Task<Result> EditGradeAsync(NewGradeDTO newGrade);
    }
}
