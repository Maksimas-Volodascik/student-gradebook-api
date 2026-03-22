using StudentGradebookApi.DTOs.Grades;
using StudentGradebookApi.Models;
using StudentGradebookApi.Repositories.Main;

namespace StudentGradebookApi.Repositories.GradesRepository
{
    public interface IGradesRepository : IRepositoryBase<Grades>
    {
        Task<IEnumerable<StudentGradesBySubjectDto>> GetStudentGradesBySubjectId(GradesQueryDto queryDto);
        Task<IEnumerable<StudentGradesBySubjectDto>> GetStudentGradesByStudentId();
        Task<Grades> GetGradeByDateAndEnrollmentId(DateTime dateTime, int enrollmentId);
    }
}
