using StudentGradebookApi.DTOs.Classes;
using StudentGradebookApi.Models;
using StudentGradebookApi.Shared;

namespace StudentGradebookApi.Services.ClassesServices
{
    public interface IClassesServices
    {
        Task<Result> AddClassAsync(ClassesContentsDTO classesContentsDTO);
        Task<Result> UpdateClassAsync(int id, ClassesContentsDTO classesContentsDTO);
        Task<Result<Classes>> GetClassByIdAsync(int id);
        Task<Result<IEnumerable<Classes>>> GetAllClassesAsync(ClassesQueryDto classesQuery);
    }
}
