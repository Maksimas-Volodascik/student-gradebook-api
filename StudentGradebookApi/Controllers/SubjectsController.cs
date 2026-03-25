using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentGradebookApi.DTOs.SharedDto;
using StudentGradebookApi.DTOs.Subjects;
using StudentGradebookApi.Models;
using StudentGradebookApi.Services.SubjectsService;

namespace StudentGradebookApi.Controllers
{
    [Authorize]
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
            var response = await _subjectsService.GetAllSubjectsAsync(query);

            return Ok(response.Data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Subjects>> GetSubjectByIdAsync(int id)
        {
            var response = await _subjectsService.GetSubjectByIdAsync(id);

            if (response.IsSuccess) return Ok(response.Data);

            return response.Error.Code switch
            {
                "subject.not.found" => NotFound(response.Error.Message),
                _ => BadRequest(response.Error.Message)
            };
        }

        [Authorize(Roles = "Teacher,Admin")]
        [HttpPatch("{id}")]
        public async Task<ActionResult<Subjects>> UpdateSubjectAsync(int id, SubjectContentsDTO sujectContentsDTO)
        {
            var response = await _subjectsService.UpdateSubjectAsync(id, sujectContentsDTO);

            if (response.IsSuccess) return Ok(response);

            return response.Error.Code switch
            {
                "subject.not.found" => NotFound(response.Error.Message),
                _ => BadRequest(response.Error.Message)
            };
        }

        [Authorize(Roles = "Teacher,Admin")]
        [HttpPost]
        public async Task<ActionResult<Subjects>> AddSubjectAsync(SubjectContentsDTO sujectContentsDTO)
        {
            var response = await _subjectsService.AddSubjectAsync(sujectContentsDTO);

            if (response.IsSuccess) return Ok(response);

            return BadRequest(response.Error.Message);
        }

        [Authorize(Roles = "Teacher,Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Subjects>> DeleteSubjectAsync(int id)
        {
            var response = await _subjectsService.DeleteSubjectAsync(id);

            if (response.IsSuccess) return Ok(response);

            return BadRequest(response.Error.Message);
        }
    }
}
