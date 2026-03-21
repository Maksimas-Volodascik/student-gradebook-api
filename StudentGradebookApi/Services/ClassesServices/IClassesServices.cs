using StudentGradebookApi.DTOs.Classes;
using StudentGradebookApi.Models;
using StudentGradebookApi.Shared;

namespace StudentGradebookApi.Services.ClassesServices
{
    public interface IClassesServices
    {
        Task<Result> AddClassAsync(NewClassDto classesContentsDTO);
        Task<Result> UpdateClassAsync(int id, NewClassDto classesContentsDTO);
        Task<Result<Classes>> GetClassByIdAsync(int id);
        Task<Result<IEnumerable<ClassSubjectsDto>>> GetAllClassesAsync(ClassesQueryDto classesQuery);
    }
}
