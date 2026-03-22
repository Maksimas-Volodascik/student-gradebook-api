using StudentGradebookApi.DTOs.Grades;
using StudentGradebookApi.Models;
using StudentGradebookApi.Shared;

namespace StudentGradebookApi.Services.GradesServices
{
    public interface IGradesServices
    {
        Task<Result<IEnumerable<StudentGradesBySubjectDto>>> GetStudentGradesBySubjectId(GradesQueryDto queryDto);
        Task<Result<IEnumerable<StudentGradesBySubjectDto>>> GetStudentGradesByStudentId(int year, int month);
        Task<Result> AddGradeAsync(NewGradeDto newGrade);
        Task<Result> EditGradeAsync(NewGradeDto newGrade);
    }
}
