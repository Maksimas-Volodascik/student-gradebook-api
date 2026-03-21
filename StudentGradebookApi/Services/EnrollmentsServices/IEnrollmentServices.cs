using StudentGradebookApi.DTOs.Enrollments;
using StudentGradebookApi.DTOs.SharedDto;
using StudentGradebookApi.Models;
using StudentGradebookApi.Shared;

namespace StudentGradebookApi.Services.EnrollmentsServices
{
    public interface IEnrollmentServices
    {
        Task<Result<IEnumerable<StudentEnrollments>>> GetStudentEnrollments(int studentId, QueryDto queryDto);

        Task<Result> EnrollStudent(int classSubjectId, string studentEmail);
    }
}
