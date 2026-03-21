using StudentGradebookApi.DTOs.SharedDto;
using StudentGradebookApi.Models;
using StudentGradebookApi.Repositories.Main;

namespace StudentGradebookApi.Repositories.SubjectsRepository
{
    public interface ISubjectsRepository : IRepositoryBase<Subjects>
    {
        Task<IEnumerable<Subjects>> GetAllSubjects(QueryDto query);
    }
}
