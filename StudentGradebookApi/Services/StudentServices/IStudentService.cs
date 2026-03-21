using StudentGradebookApi.DTOs.Students;
using StudentGradebookApi.Models;
using StudentGradebookApi.Repositories.Main;
using StudentGradebookApi.Shared;

namespace StudentGradebookApi.Services.StudentServices
{
    public interface IStudentService
    {
        Task<Result<IEnumerable<StudentEnrolledSubject>>> GetAllStudentsAsync(StudentsQueryDto queryDto);
        Task<Result<Students>> GetStudentByIdAsync(int id);
        Task<Result> AddStudentAsync(NewStudent studentData);
        Task<Result> EditStudentAsync(EditStudent studentData, int id);
        Task<Result> DeleteStudentAsync(int id);
        Task<Result<Students>> GetStudentByEmailAsync(string email);
    }
}
