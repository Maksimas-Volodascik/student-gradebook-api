using StudentGradebookApi.DTOs.Students;
using StudentGradebookApi.DTOs.Users;
using StudentGradebookApi.Models;
using StudentGradebookApi.Repositories.Main;
using StudentGradebookApi.Repositories.StudentsRepository;
using StudentGradebookApi.Services.UserServices;
using StudentGradebookApi.Shared;

namespace StudentGradebookApi.Services.StudentServices
{
    public class StudentService : IStudentService
    {
        private readonly IStudentsRepository _studentsRepository;
        private readonly IUserService _userService;
        public StudentService(IUserService userService, IStudentsRepository studentsRepository)
        {
            _studentsRepository = studentsRepository;
            _userService = userService;
        }

        public async Task<Result> AddStudentAsync(NewStudent studentData)
        {
            if (studentData == null) return Result.Failure(Errors.StudentErrors.StudentDataNull);

            RegisterDto newUser = new RegisterDto();
            newUser.Email = studentData.Email;
            newUser.Password = studentData.Password;

            var registeredUser = await _userService.RegisterAsync(newUser, "Student");
            if (!registeredUser.IsSuccess) return Result.Failure(registeredUser.Error!);

            Students student = new Students();
            student.FirstName = studentData.FirstName;
            student.LastName = studentData.LastName;
            student.DateOfBirth = studentData.DateOfBirth;
            student.User = registeredUser.Data;

            await _studentsRepository.AddAsync(student);
            await _studentsRepository.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> DeleteStudentAsync(int id)
        {
            Students? studentToDelete = await _studentsRepository.GetByIdAsync(id);

            if(studentToDelete == null) return Result.Failure(Errors.StudentErrors.StudentNotFound);

            await _userService.DeleteUserAsync(studentToDelete.UserID);

            return Result.Success();
        }

        public async Task<Result> EditStudentAsync(EditStudent studentData, int id)
        {
            if (studentData == null) return Result.Failure(Errors.StudentErrors.StudentDataNull);

            if (string.IsNullOrWhiteSpace(studentData.FirstName)) return Result.Failure(Errors.StudentErrors.StudentFirstNameEmpty);

            if (string.IsNullOrWhiteSpace(studentData.LastName)) return Result.Failure(Errors.StudentErrors.StudentLastNameEmpty);

            Students student = await _studentsRepository.GetByIdAsync(id);
            if (student == null) return Result.Failure(Errors.StudentErrors.StudentNotFound);

            student.FirstName=studentData.FirstName;
            student.LastName=studentData.LastName;
            student.DateOfBirth=studentData.DateOfBirth;
            
            _studentsRepository.Update(student);
            await _studentsRepository.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result<IEnumerable<StudentEnrolledSubject>>> GetAllStudentsAsync(StudentsQueryDto queryDto)
        {
            return Result<IEnumerable<StudentEnrolledSubject>>.Success(await _studentsRepository.GetStudentEnrolledSubjects(queryDto));
        }

        public async Task<Result<Students>> GetStudentByEmailAsync(string email)
        {
            var result = await _studentsRepository.GetStudentByEmail(email);

            if (result == null) return Result<Students>.Failure(Errors.StudentErrors.StudentNotFound);

            return Result<Students>.Success(result);
        }

        public async Task<Result<Students>> GetStudentByIdAsync(int id)
        {
            var result = await _studentsRepository.GetByIdAsync(id);

            if (result == null) return Result<Students>.Failure(Errors.StudentErrors.StudentNotFound);

            return Result<Students>.Success(result);
        }
    }
}
