using StudentGradebookApi.DTOs.SharedDto;
using StudentGradebookApi.DTOs.Subjects;
using StudentGradebookApi.DTOs.Teachers;
using StudentGradebookApi.Models;
using StudentGradebookApi.Repositories.SubjectsRepository;
using StudentGradebookApi.Shared;

namespace StudentGradebookApi.Services.SubjectsService
{
    public class SubjectsService : ISubjectsService
    {
        private readonly ISubjectsRepository _subjectsRepository;
        public SubjectsService(ISubjectsRepository subjectsRepository) {
            _subjectsRepository = subjectsRepository;
        }

        public async Task<Result> AddSubjectAsync(SubjectContentsDTO subjectContentsDTO)
        {
            var valid = ValidateSubjectData(subjectContentsDTO);
            if (!valid.IsSuccess) return Result.Failure(valid.Error);

            Subjects newSubject = new Subjects {
                SubjectName = subjectContentsDTO.subjectName,
                SubjectCode = subjectContentsDTO.subjectCode,
                Description = subjectContentsDTO.description
            };

            await _subjectsRepository.AddAsync(newSubject);
            await _subjectsRepository.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> DeleteSubjectAsync(int id)
        {
            Subjects subject = await _subjectsRepository.GetByIdAsync(id);
            if (subject == null)
                return Result.Failure(Errors.SubjectErrors.SubjectNotFound);

            _subjectsRepository.Delete(subject);
            await _subjectsRepository.SaveChangesAsync();

            return Result.Success();

        }

        public async Task<Result<IEnumerable<Subjects>>> GetAllSubjectsAsync(QueryDto query)
        {
            return Result<IEnumerable<Subjects>>.Success(await _subjectsRepository.GetAllSubjects(query));
        }

        public async Task<Result<Subjects>> GetSubjectByIdAsync(int id)
        {
            Subjects subject = await _subjectsRepository.GetByIdAsync(id);
            if (subject == null) 
                return Result<Subjects>.Failure(Errors.SubjectErrors.SubjectNotFound);

            return Result<Subjects>.Success(subject);
        }

        public async Task<Result> UpdateSubjectAsync(int id, SubjectContentsDTO subjectContentsDTO)
        {
            var valid = ValidateSubjectData(subjectContentsDTO);
            if (!valid.IsSuccess) return Result.Failure(valid.Error);

            Subjects subjects = await _subjectsRepository.GetByIdAsync(id);
            if (subjects == null)
                return Result.Failure(Errors.SubjectErrors.SubjectNotFound); 

            subjects.SubjectName = subjectContentsDTO.subjectName;
            subjects.SubjectCode = subjectContentsDTO.subjectCode;
            subjects.Description = subjectContentsDTO.description;

            _subjectsRepository.Update(subjects);
            await _subjectsRepository.SaveChangesAsync();

            return Result.Success();
        }

        public Result<SubjectContentsDTO> ValidateSubjectData(SubjectContentsDTO subjectContentsDTO)
        {
            if (string.IsNullOrWhiteSpace(subjectContentsDTO.subjectCode))
                return Result<SubjectContentsDTO>.Failure(Errors.SubjectErrors.SubjectCodeMissing);

            if (string.IsNullOrWhiteSpace(subjectContentsDTO.subjectName))
                return Result<SubjectContentsDTO>.Failure(Errors.SubjectErrors.SubjectNameMissing);

            return Result<SubjectContentsDTO>.Success(subjectContentsDTO);
        }
    }
}
