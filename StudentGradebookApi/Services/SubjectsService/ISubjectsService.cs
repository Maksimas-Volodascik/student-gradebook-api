using StudentGradebookApi.DTOs.SharedDto;
using StudentGradebookApi.DTOs.Subjects;
using StudentGradebookApi.Models;
using StudentGradebookApi.Shared;

namespace StudentGradebookApi.Services.SubjectsService
{
    public interface ISubjectsService
    {
        Task<Result> AddSubjectAsync(SubjectContentsDTO sujectContentsDTO);
        Task<Result> UpdateSubjectAsync(int id, SubjectContentsDTO sujectContentsDTO);
        Task<Result<Subjects>> GetSubjectByIdAsync(int id);
        Task<Result<IEnumerable<Subjects>>> GetAllSubjectsAsync(QueryDto query);
        Task<Result> DeleteSubjectAsync(int id);
    }
}
