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

        public async Task<IEnumerable<Classes>> GetAllClasses(ClassesQueryDto classesQuery)
        {
            var query = from c in _context.Classes
                        .Skip((classesQuery.ValidPageNumber - 1) * classesQuery.ValidPageSize)
                        .Take(classesQuery.ValidPageSize)
                        select c;

            if (classesQuery.StartingYear != null) query = query.Where(c => c.AcademicYear.StartsWith(classesQuery.StartingYear));

            return await query.ToListAsync();
        }

    }
}
