using Humanizer;
using Microsoft.EntityFrameworkCore;
using StudentGradebookApi.Data;
using StudentGradebookApi.DTOs.SubjectClass;
using StudentGradebookApi.DTOs.Teachers;
using StudentGradebookApi.Models;
using StudentGradebookApi.Repositories.Main;
using System.Linq;

namespace StudentGradebookApi.Repositories.TeachersRepository
{
    public class TeachersRepository : RepositoryBase<Teachers>, ITeachersRepository
    {
        private readonly SchoolDbContext _context;
        public TeachersRepository(SchoolDbContext context): base(context) { 
            _context = context;
        }

        public async Task<IEnumerable<TeacherDTO>> GetTeachersWithSubjectsAsync(TeachersQueryDto queryDto)
        {
            var teacherQuery = from teacher in _context.Teachers
                               .Skip((queryDto.ValidPageNumber - 1) * queryDto.ValidPageSize)
                               .Take(queryDto.ValidPageSize)
                               select teacher;
                               

            var query = from teacher in teacherQuery

                        join classSubjects in _context.ClassSubjects 
                            on teacher.Id equals classSubjects.TeacherId into classSubjectGroup
                        from classSubject in classSubjectGroup.DefaultIfEmpty()

                        join subjects in _context.Subjects 
                            on classSubject != null ? classSubject.SubjectId : 0 equals subjects.Id into subjectGroup
                        from subject in subjectGroup.DefaultIfEmpty()

                        join classes in _context.Classes 
                            on classSubject != null ? classSubject.ClassId : 0 equals classes.Id into classGroup
                        from @class in classGroup.DefaultIfEmpty()

                        select new
                        {
                            Id = teacher.Id,
                            FirstName = teacher.FirstName,
                            LastName = teacher.LastName,
                            AcademicYear = @class != null ? @class.AcademicYear : null,
                            Room = @class != null ? @class.Room : 0,
                            SubjectName = subject != null ? subject.SubjectName : null,
                            SubjectCode = subject != null ? subject.SubjectCode : null
                        };

            if(queryDto.SubjectCode != null) { query = query.Where(x => x.SubjectCode == queryDto.SubjectCode); }
            
            if(queryDto.SubjectName != null) { query = query.Where(x => x.SubjectName == queryDto.SubjectName); }
            
            if(queryDto.Room != null) { query = query.Where(x => x.Room == queryDto.Room); }
            
            if(queryDto.AcademicYear != null) { query = query.Where(x => x.AcademicYear.StartsWith(queryDto.AcademicYear)); }

            var result = await query.ToListAsync();

            var groupData = result
                .GroupBy(x => new { x.Id, x.FirstName, x.LastName })
                .Select(g => new TeacherDTO
                {
                    Id = g.Key.Id,
                    FirstName = g.Key.FirstName,
                    LastName = g.Key.LastName,
                    ClassSubjects = g.Select(x => new ClassSubjectDTO
                    {
                        AcademicYear = x.AcademicYear,
                        Room = x.Room,
                        SubjectCode = x.SubjectCode,
                        SubjectName = x.SubjectName
                    }).ToList()
                }).ToList();

            return groupData;
        }

        public async Task<Teachers> GetTeacherByEmail(string email)
        {
            Teachers? teacher = await(from T in _context.Teachers
                                      join WU in _context.WebUsers
                                          on T.UserID equals WU.Id
                                      where WU.Email == email
                                      select T).FirstOrDefaultAsync();

            return teacher;
        }
    }
}
