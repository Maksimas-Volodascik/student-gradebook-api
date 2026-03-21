using StudentGradebookApi.DTOs.Students;
using StudentGradebookApi.DTOs.Teachers;
using StudentGradebookApi.Models;
using StudentGradebookApi.Shared;

namespace StudentGradebookApi.Services.TeacherServices
{
    public interface ITeacherService
    {
        Task<Result<IEnumerable<TeacherDTO>>> GetAllTeachersAsync(TeachersQueryDto queryDto);
        Task<Result<Teachers>> GetTeacherByIdAsync(int id);
        Task<Result> AddTeacherAsync(TeacherRequestDTO teacherData);
        Task<Result> EditTeacherAsync(int teacherId, TeacherRequestDTO teacher);
        Task<Result> DeleteTeacherAsync(int id);
    }
}
