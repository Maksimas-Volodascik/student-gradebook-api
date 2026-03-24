using StudentGradebookApi.DTOs.ClassSubjects;
using StudentGradebookApi.DTOs.SubjectClass;
using StudentGradebookApi.Models;
using StudentGradebookApi.Shared;

namespace StudentGradebookApi.Services.SubjectClassServices
{
    public interface IClassSubjectsService
    {
        Task<Result> AssignSubjectToClassAsync (CombineClassSubjectDto combineClassSubjectDTO);
        Task<Result> RemoveSubjectClassAsync(int classSubjectsId);
        Task<Result> EditSubjectClassTeacher(int classSubjectsId, int teacherId);  //Function to change class teacher
        Task<Result<IEnumerable<ClassSubjectDto>>> GetAllClassSubjectsAsync();
        Task<Result<ClassSubjects>> GetClassSubjectsByIdAsync(int classSubjectsId);
    }
}
