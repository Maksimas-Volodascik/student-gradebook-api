using StudentGradebookApi.DTOs.ClassSubjects;
using StudentGradebookApi.DTOs.SubjectClass;
using StudentGradebookApi.Models;
using StudentGradebookApi.Repositories.ClassesRepository;
using StudentGradebookApi.Repositories.ClassSubjectsRepository;
using StudentGradebookApi.Repositories.Main;
using StudentGradebookApi.Repositories.StudentsRepository;
using StudentGradebookApi.Repositories.SubjectsRepository;
using StudentGradebookApi.Repositories.TeachersRepository;
using StudentGradebookApi.Services.ClassesServices;
using StudentGradebookApi.Services.SubjectsService;
using StudentGradebookApi.Services.TeacherServices;
using StudentGradebookApi.Services.UserServices;
using StudentGradebookApi.Shared;
using System.Collections.Generic;

namespace StudentGradebookApi.Services.SubjectClassServices
{
    public class ClassSubjectsService : IClassSubjectsService
    {
        private readonly IClassSubjectsRepository _classSubjectsRepository;
        private readonly ITeacherService _teacherService;
        private readonly ISubjectsService _subjectsService;
        private readonly IClassesServices _classesServices;

        public ClassSubjectsService(IClassSubjectsRepository classSubjectsRepository, ITeacherService teacherService, ISubjectsService subjectsService, IClassesServices classesServices)
        {
            _classSubjectsRepository = classSubjectsRepository;
            _teacherService = teacherService;
            _subjectsService= subjectsService;
            _classesServices = classesServices;
        }

        public async Task<Result> AssignSubjectToClassAsync(CombineClassSubjectDto combineClassSubjectDTO)
        {
            var subject = await _subjectsService.GetSubjectByIdAsync(combineClassSubjectDTO.SubjectId);
            var classes = await _classesServices.GetClassByIdAsync(combineClassSubjectDTO.ClassId);

            if (!subject.IsSuccess) return Result.Failure(subject.Error);

            if (!classes.IsSuccess) return Result.Failure(classes.Error);

            ClassSubjects classSubjects = new ClassSubjects();
            classSubjects.SubjectId = combineClassSubjectDTO.SubjectId;
            classSubjects.ClassId = combineClassSubjectDTO.ClassId;

            await _classSubjectsRepository.AddAsync(classSubjects);
            await _classSubjectsRepository.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> EditSubjectClassTeacher(int classSubjectsId, int teacherId)
        {
            var teacher = await _teacherService.GetTeacherByIdAsync(teacherId);
            var classSubject = await GetClassSubjectsByIdAsync(classSubjectsId);

            if (!teacher.IsSuccess) return Result.Failure(teacher.Error);
                
            if (!classSubject.IsSuccess) return Result.Failure(classSubject.Error);

            classSubject.Data.Teachers = teacher.Data;
            classSubject.Data.TeacherId = teacherId;

            _classSubjectsRepository.Update(classSubject.Data);
            await _classSubjectsRepository.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result<ClassSubjects>> GetClassSubjectsByIdAsync(int classSubjectsId)
        {
            ClassSubjects? classSubject = await _classSubjectsRepository.GetByIdAsync(classSubjectsId);
            if (classSubject == null) return Result<ClassSubjects>.Failure(Errors.ClassSubjectErrors.ClassSubjectNotFound);

            return Result<ClassSubjects>.Success(classSubject);
        }

        public async Task<Result<IEnumerable<ClassSubjectDto>>> GetAllClassSubjectsAsync()
        {
            return Result<IEnumerable<ClassSubjectDto>>.Success(await _classSubjectsRepository.GetAllClassSubjectsAsync());
        }

        public async Task<Result> RemoveSubjectClassAsync(int classSubjectsId)
        {
            ClassSubjects? classSubject = await _classSubjectsRepository.GetByIdAsync(classSubjectsId);
            if (classSubject == null) return Result<ClassSubjects>.Failure(Errors.ClassSubjectErrors.ClassSubjectNotFound);

            _classSubjectsRepository.Delete(classSubject);
            await _classSubjectsRepository.SaveChangesAsync();
            
            return Result.Success();
        }
    }
}
