using Microsoft.EntityFrameworkCore;
using StudentGradebookApi.Data;
using StudentGradebookApi.DTOs.SharedDto;
using StudentGradebookApi.DTOs.SubjectClass;
using StudentGradebookApi.Models;
using StudentGradebookApi.Repositories.Main;

namespace StudentGradebookApi.Repositories.SubjectsRepository
{
    public class SubjectsRepository : RepositoryBase<Subjects>, ISubjectsRepository
    {
        private readonly SchoolDbContext _context;
        public SubjectsRepository(SchoolDbContext context) : base(context) {
            _context = context;
        }

        public async Task<IEnumerable<Subjects>> GetAllSubjects(QueryDto query)
        {
            var subjectsQuery = from subject in _context.Subjects
                           .Skip((query.ValidPageNumber - 1) * query.ValidPageSize)
                           .Take(query.ValidPageSize)
                                select subject;

            return await subjectsQuery.ToListAsync();
        }
    }
}
