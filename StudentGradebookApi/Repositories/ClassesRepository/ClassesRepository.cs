using Microsoft.EntityFrameworkCore;
using StudentGradebookApi.Data;
using StudentGradebookApi.DTOs.Classes;
using StudentGradebookApi.Models;
using StudentGradebookApi.Repositories.Main;

namespace StudentGradebookApi.Repositories.ClassesRepository
{
    public class ClassesRepository : RepositoryBase<Classes>, IClassesRepository
    {
        private readonly SchoolDbContext _context;
        public ClassesRepository(SchoolDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ClassSubjectsDto>> GetAllClasses(ClassesQueryDto classesQuery)
        {
            var classQuery = await (from classes in _context.Classes
                        .Where(c => (classesQuery.StartingYear == null || c.AcademicYear.StartsWith(classesQuery.StartingYear)) &&
                        (classesQuery.Room == null || c.Room == classesQuery.Room))

                        join classSubjects in _context.ClassSubjects
                            on classes.Id equals classSubjects.ClassId into classSubjects
                        from classSubject in classSubjects.DefaultIfEmpty()

                        join subjects in _context.Subjects
                            on classSubject.SubjectId equals subjects.Id into subjects
                        from subject in subjects.DefaultIfEmpty()

                        .Skip((classesQuery.ValidPageNumber - 1) * classesQuery.ValidPageSize)
                        .Take(classesQuery.ValidPageSize)

                        select new ClassSubjectsDto{ 
                            Id = classes.Id,
                            AcademicYear = classes.AcademicYear,
                            Room = classes.Room,
                            SubjectName = subject != null ? subject.SubjectName : ""
                        }).ToListAsync();


            return classQuery;
        }

    }
}
