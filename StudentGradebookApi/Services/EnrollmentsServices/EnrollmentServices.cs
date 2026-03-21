using StudentGradebookApi.DTOs.Enrollments;
using StudentGradebookApi.DTOs.SharedDto;
using StudentGradebookApi.Models;
using StudentGradebookApi.Repositories.ClassSubjectsRepository;
using StudentGradebookApi.Repositories.EnrollmentsRepository;
using StudentGradebookApi.Repositories.StudentsRepository;
using StudentGradebookApi.Services.StudentServices;
using StudentGradebookApi.Services.SubjectClassServices;
using StudentGradebookApi.Shared;

namespace StudentGradebookApi.Services.EnrollmentsServices
{
    public class EnrollmentServices : IEnrollmentServices
    {
        private readonly IEnrollmentsRepository _enrollmentsRepository;
        private readonly IStudentService _studentService;
        private readonly IClassSubjectsService _classSubjectsService;
        public EnrollmentServices(IEnrollmentsRepository enrollmentsRepository, IStudentService studentService, IClassSubjectsService classSubjectsService) {
            _enrollmentsRepository = enrollmentsRepository;
            _studentService = studentService;
            _classSubjectsService = classSubjectsService;
        }

        public async Task<Result> EnrollStudent(int classSubjectId, string studentEmail)
        {
            Result<Students> student = await _studentService.GetStudentByEmailAsync(studentEmail);
            if (!student.IsSuccess) return Result.Failure(student.Error!);

            Result<ClassSubjects> classSubject = await _classSubjectsService.GetClassSubjectsByIdAsync(classSubjectId);
            if (!classSubject.IsSuccess) return Result.Failure(classSubject.Error!);

            Enrollments enrollment = new Enrollments();
            enrollment.Status = "Enrolled";
            enrollment.StudentID = student.Data.Id;
            enrollment.ClassSubjectId = classSubject.Data.Id;

            await _enrollmentsRepository.AddAsync(enrollment);
            await _enrollmentsRepository.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result<IEnumerable<StudentEnrollments>>> GetStudentEnrollments(int studentId, QueryDto queryDto)
        {
            return Result<IEnumerable<StudentEnrollments>>.Success(await _enrollmentsRepository.GetStudentEnrollmentsAsync(studentId, queryDto));
        }


    }
}
