using Microsoft.EntityFrameworkCore;
using StudentGradebookApi.Data;
using StudentGradebookApi.DTOs.Students;
using StudentGradebookApi.DTOs.SubjectClass;
using StudentGradebookApi.Models;
using StudentGradebookApi.Repositories.ClassesRepository;
using StudentGradebookApi.Repositories.Main;

namespace StudentGradebookApi.Repositories.ClassSubjectsRepository
{
    public class ClassSubjectsRepository : RepositoryBase<ClassSubjects>, IClassSubjectsRepository
    {
        private readonly SchoolDbContext _context;
        public ClassSubjectsRepository(SchoolDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ClassSubjectDTO>> GetAllClassSubjectsAsync()
        {
            var query = from CS in _context.ClassSubjects
                        join C in _context.Classes
                            on CS.ClassId equals C.Id
                        join S in _context.Subjects
                            on CS.SubjectId equals S.Id
                        select new ClassSubjectDTO
                        {
                            AcademicYear = C.AcademicYear,
                            Room = C.Room,
                            SubjectName = S.SubjectName,
                            SubjectCode = S.SubjectCode,
                        };

            return await query.ToListAsync();
        }
    }
}
