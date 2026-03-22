using StudentGradebookApi.DTOs.SubjectClass;
using StudentGradebookApi.Models;
using StudentGradebookApi.Repositories.Main;

namespace StudentGradebookApi.Repositories.ClassSubjectsRepository
{
    public interface IClassSubjectsRepository : IRepositoryBase<ClassSubjects>
    {
        Task<IEnumerable<ClassSubjectDto>> GetAllClassSubjectsAsync();
    }
}
