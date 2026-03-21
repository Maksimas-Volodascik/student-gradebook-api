using Microsoft.EntityFrameworkCore;
using StudentGradebookApi.Data;
using StudentGradebookApi.DTOs.Students;
using StudentGradebookApi.DTOs.Subjects;
using StudentGradebookApi.Models;
using StudentGradebookApi.Repositories.Main;
using System;

namespace StudentGradebookApi.Repositories.StudentsRepository
{
    public class StudentsRepository : RepositoryBase<Students> , IStudentsRepository
    {
        private readonly SchoolDbContext _context;
        public StudentsRepository(SchoolDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StudentEnrolledSubject>> GetStudentEnrolledSubjects(StudentsQueryDto queryDto)
        {
            var studentsQuery = from student in _context.Students
                           .Where(s => (queryDto.FirstName == null || s.FirstName.StartsWith(queryDto.FirstName)) &&
                           (queryDto.LastName == null || s.LastName.StartsWith(queryDto.LastName)) &&
                           (queryDto.DateOfBirth == null || s.DateOfBirth.Date == queryDto.DateOfBirth.Value.Date) &&
                           (queryDto.EnrollmentDate == null || s.EnrollmentDate.Date == queryDto.EnrollmentDate.Value.Date) &&
                           (queryDto.Status == null || s.Status.StartsWith(queryDto.Status)))
                           .Skip((queryDto.ValidPageNumber - 1) * queryDto.ValidPageSize)
                           .Take(queryDto.ValidPageSize)
                           select student;
            
            var query = await (from students in studentsQuery
                               join enrollments in _context.Enrollments
                                   on students.Id equals enrollments.StudentID into enrollments
                               from enrollment in enrollments.DefaultIfEmpty()

                               join classSubjects in _context.ClassSubjects
                                   on enrollment.ClassSubjectId equals classSubjects.Id into classSubjects
                               from classSubject in classSubjects.DefaultIfEmpty()

                               join subjects in _context.Subjects
                                   on classSubject.SubjectId equals subjects.Id into subjects
                               from subject in subjects.DefaultIfEmpty()

                               select new
                               {
                                   Id = students.Id,
                                   FirstName = students.FirstName,
                                   LastName = students.LastName,
                                   DateOfBirth = students.DateOfBirth,
                                   EnrollmentDate = students.EnrollmentDate,
                                   Status = students.Status,
                                   SubjectName = subject.SubjectName
                               }).ToListAsync();

            var groupData = query.GroupBy(x => new { x.Id, x.FirstName, x.LastName, x.DateOfBirth, x.EnrollmentDate, x.Status })
                                .Select(g => new StudentEnrolledSubject
                                {
                                    Id = g.Key.Id,
                                    FirstName = g.Key.FirstName,
                                    LastName = g.Key.LastName,
                                    DateOfBirth = g.Key.DateOfBirth,
                                    EnrollmentDate = g.Key.EnrollmentDate,
                                    Status = g.Key.Status,
                                    Subjects = g.Select(s => s.SubjectName != null ? s.SubjectName : "")
                                                .ToList()
                                }).ToList();

            return groupData;
        }
        public async Task<Students> GetStudentByEmail(string email)
        {
            Students? student = await (from S in _context.Students
                                 join WU in _context.WebUsers
                                     on S.UserID equals WU.Id
                                 where WU.Email == email
                                 select S).FirstOrDefaultAsync();

            return student;
        }
    }
}
