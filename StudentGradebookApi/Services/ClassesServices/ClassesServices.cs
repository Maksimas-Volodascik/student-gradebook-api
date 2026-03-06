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

        public async Task<Result> AddClassAsync(ClassesContentsDTO classesContentsDTO)
        {
            Classes newClass = new Classes();
            newClass.Room = classesContentsDTO.room;
            newClass.AcademicYear = classesContentsDTO.academicYear;

            await _classesRepository.AddAsync(newClass);
            await _classesRepository.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result<IEnumerable<Classes>>> GetAllClassesAsync()
        {
            return Result<IEnumerable<Classes>>.Success(await _classesRepository.GetAllAsync());
        }

        public async Task<Result<Classes>> GetClassByIdAsync(int id)
        {
            Classes? classes = await _classesRepository.GetByIdAsync(id);
            if (classes == null) return Result<Classes>.Failure(Errors.ClassesErrors.ClassNotFound);

            return Result<Classes>.Success(classes);
        }

        public async Task<Result<IEnumerable<Classes>>> GetClassesByYearAsync(string academicYear)
        {
            return Result<IEnumerable<Classes>>.Success(await _classesRepository.GetClassesByYearAsync(academicYear));
        }

        public async Task<Result> UpdateClassAsync(int id, ClassesContentsDTO classesContentsDTO)
        {
            var valid = ValidateTeacherData(classesContentsDTO);
            if (!valid.IsSuccess) return Result.Failure(valid.Error!);

            var classes = await GetClassByIdAsync(id);
            if (!classes.IsSuccess) return Result.Failure(classes.Error!);

            classes.Data.Room = classesContentsDTO.room;
            classes.Data.AcademicYear = classesContentsDTO.academicYear;

            _classesRepository.Update(classes.Data);
            await _classesRepository.SaveChangesAsync();

            return Result.Success();
        }

        public Result ValidateTeacherData(ClassesContentsDTO classesContentsDTO)
        {
            if (Regex.IsMatch(classesContentsDTO.academicYear, @"^(\d{4})-(\d{4})$") && int.Parse(classesContentsDTO.academicYear.Split('-')[1]) == int.Parse(classesContentsDTO.academicYear.Split('-')[0]) + 1)
                return Result.Failure(Errors.ClassesErrors.InvalidClassesAcademicYear);

            if (classesContentsDTO.room < 100 || classesContentsDTO.room > 1000)
                return Result.Failure(Errors.ClassesErrors.InvalidClassesRoom);

            return Result.Success();
        }
    }
}
