using Microsoft.AspNetCore.Mvc;
using StudentGradebookApi.DTOs.SharedDto;
using StudentGradebookApi.DTOs.Subjects;
using StudentGradebookApi.Models;
using StudentGradebookApi.Services.SubjectsService;

namespace StudentGradebookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly ISubjectsService _subjectsService;
        public SubjectsController(ISubjectsService subjectsService)
        {
            _subjectsService = subjectsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subjects>>> GetAllSubjectsAsync([FromQuery] QueryDto query)
        {
            var subjects = await _subjectsService.GetAllSubjectsAsync(query);

            return Ok(subjects.Data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Subjects>> GetSubjectByIdAsync(int id)
        {
            var subjects = await _subjectsService.GetSubjectByIdAsync(id);
            return Ok(subjects);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<Subjects>> UpdateSubjectAsync(int id, SubjectContentsDTO sujectContentsDTO)
        {
            var subjects = await _subjectsService.UpdateSubjectAsync(id, sujectContentsDTO);
            return Ok(subjects);
        }

        [HttpPost]
        public async Task<ActionResult<Subjects>> AddSubjectAsync(SubjectContentsDTO sujectContentsDTO)
        {
            var subjects = await _subjectsService.AddSubjectAsync(sujectContentsDTO);
            return Ok(subjects);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Subjects>> DeleteSubjectAsync(int id)
        {
            var subjects = await _subjectsService.DeleteSubjectAsync(id);
            return Ok(subjects);
        }
    }
}
