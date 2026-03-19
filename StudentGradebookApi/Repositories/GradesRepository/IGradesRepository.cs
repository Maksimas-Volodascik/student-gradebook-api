using StudentGradebookApi.DTOs.Grades;
using StudentGradebookApi.Models;
using StudentGradebookApi.Repositories.Main;

namespace StudentGradebookApi.Repositories.GradesRepository
{
    public interface IGradesRepository : IRepositoryBase<Grades>
    {
        Task<IEnumerable<StudentGradesBySubjectDTO>> GetStudentGradesBySubjectId(GradesQueryDto queryDto);
        Task<IEnumerable<StudentGradesBySubjectDTO>> GetStudentGradesByStudentId();
        Task<Grades> GetGradeByDateAndEnrollmentId(DateTime dateTime, int enrollmentId);
    }
}
