using Microsoft.EntityFrameworkCore;
using StudentGradebookApi.Data;
using StudentGradebookApi.DTOs.SubjectClass;
using StudentGradebookApi.DTOs.Teachers;
using StudentGradebookApi.Models;
using StudentGradebookApi.Repositories.Main;

namespace StudentGradebookApi.Repositories.TeachersRepository
{
    public class TeachersRepository : RepositoryBase<Teachers>, ITeachersRepository
    {
        private readonly SchoolDbContext _context;
        public TeachersRepository(SchoolDbContext context): base(context) { 
            _context = context;
        }

        public async Task<IEnumerable<TeacherDTO>> GetTeachersWithSubjectsAsync()
        {
            var query = await (from T in _context.Teachers
                        join CS in _context.ClassSubjects on T.Id equals CS.TeacherId 
                        join Subject in _context.Subjects on CS.SubjectId equals Subject.Id 
                        join Class in _context.Classes on CS.ClassId equals Class.Id 
                        select new
                            {
                                Id = T.Id,
                                FirstName = T.FirstName,
                                LastName = T.LastName,
                                AcademicYear = Class.AcademicYear,
                                Room = Class.Room,
                                SubjectName = Subject.SubjectName,
                                SubjectCode = Subject.SubjectCode
                            }
                        ).ToListAsync();

            var groupData = query
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
