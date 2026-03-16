using StudentGradebookApi.DTOs.Classes;
using StudentGradebookApi.Models;
using StudentGradebookApi.Repositories.Main;

namespace StudentGradebookApi.Repositories.ClassesRepository
{
    public interface IClassesRepository : IRepositoryBase<Classes>
    {
        Task<IEnumerable<Classes>> GetAllClasses(ClassesQueryDto classesQuery);
    }
}
