using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentGradebookApi.Data;
using StudentGradebookApi.DTOs.Enrollments;
using StudentGradebookApi.DTOs.Grades;
using StudentGradebookApi.Models;
using StudentGradebookApi.Repositories.Main;
using System.Security.Claims;

namespace StudentGradebookApi.Repositories.GradesRepository
{
    public class GradesRepository : RepositoryBase<Grades>, IGradesRepository
    {
        private readonly SchoolDbContext _context;
        public GradesRepository(SchoolDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Grades> GetGradeByDateAndEnrollmentId(DateTime dateTime, int enrollmentId)
        {
            Grades? query = await _context.Grades.FirstOrDefaultAsync(g => g.GradingDay == dateTime && g.EnrollmentId == enrollmentId);
                        
            return query;
        }

        public async Task<IEnumerable<StudentGradesBySubjectDto>> GetStudentGradesByStudentId()
        {
            return null;
        }

        public async Task<IEnumerable<StudentGradesBySubjectDto>> GetStudentGradesBySubjectId(GradesQueryDto queryDto)
        {
            var studentQuery = from student in _context.Students
                               .Skip((queryDto.ValidPageNumber - 1) * queryDto.ValidPageSize)
                               .Take(queryDto.ValidPageSize)
                               select student;

            var gradesQuery = from student in studentQuery

                              join enrollments in _context.Enrollments
                                  on student.Id equals enrollments.StudentID into enrollments
                              from enrollment in enrollments
                              .DefaultIfEmpty()
                                  //.Where(e => e.ClassSubjectId == queryDto.ClassSubjectId)

                              join grades in _context.Grades
                                   on enrollment.Id equals grades.EnrollmentId into grades
                              from grade in grades
                              .Where(g => g.GradingDate.Year == queryDto.GradingYear && g.GradingDate.Month == queryDto.GradingMonth)
                              .DefaultIfEmpty()

                              select new
                              {
                                FirstName = student.FirstName,
                                LastName = student.LastName,
                                ClassSubjectId = enrollment != null ? enrollment.ClassSubjectId : 0,
                                EnrollmentId = enrollment != null ? enrollment.Id : 0,
                                Score = grade != null ? grade.Score : 0,
                                Grade_Type = grade != null ? grade.Grade_Type : null,
                                GradingDate = grade != null ? grade.GradingDate : default
                              };

            var result = await gradesQuery.ToListAsync();

            var groupData = result
                            .GroupBy(x => new {x.FirstName, x.LastName, x.ClassSubjectId, x.EnrollmentId})
                            .Select(g => new StudentGradesBySubjectDto
                            {
                                FirstName = g.Key.FirstName,
                                LastName = g.Key.LastName,
                                ClassSubjectId = g.Key.ClassSubjectId,
                                EnrollmentId = g.Key.EnrollmentId,
                                Grades = g.Select(x => new GradesListDto
                                {
                                    Score = x.Score,
                                    Grade_Type = x.Grade_Type,
                                    GradingDate = x.GradingDate,
                                }).ToList(),
                            }).ToList();
            
            return groupData; 
        }
    }
}
