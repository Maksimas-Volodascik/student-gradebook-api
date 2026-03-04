
using StudentGradebookApi.DTOs.Grades;
using StudentGradebookApi.DTOs.Teachers;
using StudentGradebookApi.Models;
using StudentGradebookApi.Repositories.GradesRepository;
using StudentGradebookApi.Shared;

namespace StudentGradebookApi.Services.GradesServices
{
    public class GradesServices : IGradesServices
    {
        private readonly IGradesRepository _gradesRepository;
        public GradesServices(IGradesRepository gradesRepository)
        {
            _gradesRepository = gradesRepository;
        }

        public async Task<Result> EditGradeAsync(NewGradeDTO newGrade)
        {
            var valid = ValidateGradeData(newGrade);
            if (!valid.IsSuccess) return Result.Failure(valid.Error);

            Grades grade = await _gradesRepository.GetGradeByDateAndEnrollmentId(newGrade.gradingDate.Date, newGrade.enrollmentId);
            //TODO: test grade that does not exist
            grade.Grade_Type = newGrade.gradeType;
            grade.Score = newGrade.score;

            _gradesRepository.Update(grade);
            await _gradesRepository.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> AddGradeAsync(NewGradeDTO newGrade)
        {
            var valid = ValidateGradeData(newGrade);
            if (!valid.IsSuccess) return Result.Failure(valid.Error!);

            Grades createGrade = new Grades();
            createGrade.GradingDate = newGrade.gradingDate;
            createGrade.Grade_Type = newGrade.gradeType;
            createGrade.Score = newGrade.score;
            createGrade.EnrollmentId = newGrade.enrollmentId;

            await _gradesRepository.AddAsync(createGrade);
            await _gradesRepository.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result<IEnumerable<StudentGradesBySubjectDTO>>> GetStudentGradesByStudentId(int year, int month)
        {
            //TODO: add call for student grades by ID
            return Result<IEnumerable<StudentGradesBySubjectDTO>>.Success(await _gradesRepository.GetStudentGradesBySubjectId(year, month, 1));
        }

        public async Task<Result<IEnumerable<StudentGradesBySubjectDTO>>> GetStudentGradesBySubjectId(int year, int month, int classSubjectId)
        {
            return Result<IEnumerable<StudentGradesBySubjectDTO>>.Success(await _gradesRepository.GetStudentGradesBySubjectId(year, month, classSubjectId));
        }

        public Result ValidateGradeData(NewGradeDTO grade)
        {
            if (grade.score > 10)
                return Result.Failure(Errors.GradeErrors.ScoreOutOfRange);

            var allowedGradeTypes = new[] { "default", "test", "exam", "project" };
            if (string.IsNullOrEmpty(grade.gradeType) || !allowedGradeTypes.Contains(grade.gradeType))
                return Result.Failure(Errors.GradeErrors.GradeTypeInvalid); 

            if (grade.gradingDate == default(DateTime))
                return Result.Failure(Errors.GradeErrors.GradingDateInvalid); 

            if (grade.enrollmentId <= 0)
                return Result.Failure(Errors.GradeErrors.EnrollmentIdInvalid); 

            return Result.Success();
        }
    }
}
