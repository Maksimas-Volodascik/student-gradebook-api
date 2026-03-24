using StudentGradebookApi.DTOs.Students;
using StudentGradebookApi.DTOs.Teachers;
using StudentGradebookApi.Models;
using StudentGradebookApi.Shared;

namespace StudentGradebookApi.Services.TeacherServices
{
    public interface ITeacherService
    {
        Task<Result<IEnumerable<TeacherDto>>> GetAllTeachersAsync(TeachersQueryDto queryDto);
        Task<Result<Teachers>> GetTeacherByIdAsync(int id);
        Task<Result> AddTeacherAsync(TeacherRequestDto teacherData);
        Task<Result> EditTeacherAsync(int teacherId, TeacherRequestDto teacher);
        Task<Result> DeleteTeacherAsync(int id);
    }
}
