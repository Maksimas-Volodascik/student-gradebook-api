using Microsoft.AspNetCore.Http.HttpResults;
using StudentGradebookApi.DTOs.Teachers;
using StudentGradebookApi.DTOs.Users;
using StudentGradebookApi.Models;
using StudentGradebookApi.Repositories.Main;
using StudentGradebookApi.Repositories.TeachersRepository;
using StudentGradebookApi.Services.SubjectClassServices;
using StudentGradebookApi.Services.UserServices;
using StudentGradebookApi.Shared;
using System.ComponentModel.DataAnnotations;

namespace StudentGradebookApi.Services.TeacherServices
{
    public class TeacherService : ITeacherService
    {
        private readonly ITeachersRepository _teachersRepository;
        private readonly IUserService _userService;
        private readonly IClassSubjectsService _classSubjectsService;
        public TeacherService(ITeachersRepository teachersRepository, IUserService userService, IClassSubjectsService classSubjectsService) { 
            _teachersRepository = teachersRepository;
            _userService = userService;
            _classSubjectsService = classSubjectsService;
        }

        public async Task<Result> AddTeacherAsync(TeacherRequestDTO teacherData)
        {
            var validateTeacherData = ValidateTeacherData(teacherData);
            if (!validateTeacherData.IsSuccess) 
                return Result<Teachers>.Failure(validateTeacherData.Error!);

            NewUserDTO newUser = new NewUserDTO();
            newUser.Email = teacherData.Email;
            newUser.Password = teacherData.Password;
            newUser.Role = "teacher";

            var registeredUser = await _userService.RegisterAsync(newUser);
            if (!registeredUser.IsSuccess) return Result<Teachers>.Failure(registeredUser.Error);

            Teachers newTeacher = new Teachers
            {
                FirstName = teacherData.FirstName,
                LastName = teacherData.LastName,
                UserID = registeredUser.Data.Id
            };

            await _teachersRepository.AddAsync(newTeacher);
            await _teachersRepository.SaveChangesAsync();

            Teachers teacher = await _teachersRepository.GetTeacherByEmail(teacherData.Email);
            if (teacher != null) 
                await _classSubjectsService.EditSubjectClassTeacher(teacherData.ClassSubjectId, teacher.Id);

            return Result.Success();
        }

        public async Task<Result> EditTeacherAsync(int teacherId, TeacherRequestDTO teacherData)
        {
            var validation = ValidateTeacherData(teacherData);
            if (!validation.IsSuccess)
                return Result<Teachers>.Failure(validation.Error!);

            Teachers updateTeacher = await _teachersRepository.GetByIdAsync(teacherId);
            if(updateTeacher == null) return Result<Teachers>.Failure(Errors.TeacherErrors.TeacherNotFound);

            updateTeacher.FirstName = teacherData.FirstName;
            updateTeacher.LastName = teacherData.LastName;
            
            await _classSubjectsService.EditSubjectClassTeacher(teacherData.ClassSubjectId, teacherId);

            _teachersRepository.Update(updateTeacher);
            await _teachersRepository.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result<Teachers>> GetTeacherByIdAsync(int id)
        {
            var response = await _teachersRepository.GetByIdAsync(id);
            if (response == null) return Result<Teachers>.Failure(Errors.TeacherErrors.TeacherNotFound);

            return Result<Teachers>.Success(response);
        }

        public async Task<Result<IEnumerable<TeacherDTO>>> GetAllTeachersAsync()
        {
            var response = await _teachersRepository.GetTeachersWithSubjectsAsync();
            return Result<IEnumerable<TeacherDTO>>.Success(response);
        }

        public async Task<Result> DeleteTeacherAsync(int id)
        {
            var teacher = await _teachersRepository.GetByIdAsync(id);
            if (teacher == null)
                return Result.Failure(Errors.TeacherErrors.TeacherNotFound);

            var response = await _userService.DeleteUserAsync(teacher.UserID);
            if (!response.IsSuccess) return Result.Failure(response.Error!);

            return Result.Success();
        }

        public Result<TeacherRequestDTO> ValidateTeacherData(TeacherRequestDTO teacherData)
        {
            if (string.IsNullOrWhiteSpace(teacherData.FirstName))
                return Result<TeacherRequestDTO>.Failure(Errors.TeacherErrors.FirstNameMissing);

            if (string.IsNullOrWhiteSpace(teacherData.LastName))
                return Result<TeacherRequestDTO>.Failure(Errors.TeacherErrors.LastNameMissing);

            return Result<TeacherRequestDTO>.Success(teacherData);
        }
    }
}
