using StudentGradebookApi.DTOs.Students;
using StudentGradebookApi.Models;
using StudentGradebookApi.Repositories.Main;

namespace StudentGradebookApi.Repositories.StudentsRepository
{
    public interface IStudentsRepository : IRepositoryBase<Students>
    {
        Task<IEnumerable<StudentEnrolledSubject>> GetStudentEnrolledSubjects(StudentsQueryDto queryDto);

        Task<Students> GetStudentByEmail(string email);
    }
}
