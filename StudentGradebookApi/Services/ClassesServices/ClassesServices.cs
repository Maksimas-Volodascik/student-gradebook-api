using StudentGradebookApi.DTOs.Classes;
using StudentGradebookApi.DTOs.Teachers;
using StudentGradebookApi.Models;
using StudentGradebookApi.Repositories.ClassesRepository;
using StudentGradebookApi.Shared;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace StudentGradebookApi.Services.ClassesServices
{
    public class ClassesServices : IClassesServices
    {
        private readonly IClassesRepository _classesRepository;
        public ClassesServices(IClassesRepository classesRepository) { 
            _classesRepository = classesRepository;
        }

        public async Task<Result> AddClassAsync(NewClassDto classesContentsDTO)
        {
            Classes newClass = new Classes();
            newClass.Room = classesContentsDTO.Room;
            newClass.AcademicYear = classesContentsDTO.AcademicYear;

            await _classesRepository.AddAsync(newClass);
            await _classesRepository.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result<IEnumerable<ClassSubjectsDto>>> GetAllClassesAsync(ClassesQueryDto classesQuery)
        {
            return Result<IEnumerable<ClassSubjectsDto>>.Success(await _classesRepository.GetAllClasses(classesQuery));
        }

        public async Task<Result<Classes>> GetClassByIdAsync(int id)
        {
            Classes? classes = await _classesRepository.GetByIdAsync(id);
            if (classes == null) return Result<Classes>.Failure(Errors.ClassesErrors.ClassNotFound);

            return Result<Classes>.Success(classes);
        }

        public async Task<Result> UpdateClassAsync(int id, NewClassDto classesContentsDTO)
        {
            var valid = ValidateTeacherData(classesContentsDTO);
            if (!valid.IsSuccess) return Result.Failure(valid.Error!);

            var classes = await GetClassByIdAsync(id);
            if (!classes.IsSuccess) return Result.Failure(classes.Error!);

            classes.Data.Room = classesContentsDTO.Room;
            classes.Data.AcademicYear = classesContentsDTO.AcademicYear;

            _classesRepository.Update(classes.Data);
            await _classesRepository.SaveChangesAsync();

            return Result.Success();
        }

        public Result ValidateTeacherData(NewClassDto classesContentsDTO)
        {
            if (!Regex.IsMatch(classesContentsDTO.AcademicYear, @"^(\d{4})-(\d{4})$") || 
                int.Parse(classesContentsDTO.AcademicYear.Split('-')[1]) != int.Parse(classesContentsDTO.AcademicYear.Split('-')[0]) + 1)
                return Result.Failure(Errors.ClassesErrors.InvalidClassesAcademicYear);

            if (classesContentsDTO.Room < 100 || classesContentsDTO.Room > 1000)
                return Result.Failure(Errors.ClassesErrors.InvalidClassesRoom);

            return Result.Success();
        }
    }
}
