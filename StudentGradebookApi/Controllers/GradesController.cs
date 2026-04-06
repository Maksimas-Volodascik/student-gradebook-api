using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentGradebookApi.DTOs.Grades;
using StudentGradebookApi.Models;
using StudentGradebookApi.Repositories.GradesRepository;
using StudentGradebookApi.Services.GradesServices;
using StudentGradebookApi.Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StudentGradebookApi.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class GradesController : ControllerBase
    {
        private readonly IGradesServices _gradesService;
        public GradesController(IGradesServices gradesServices) { 
            _gradesService = gradesServices;
        }

        [Authorize(Roles = "Student,Teacher,Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentGradesBySubjectDto>>> GetStudentGrades([FromQuery] GradesQueryDto queryDto)
        {
            var studentGrades = await _gradesService.GetStudentGradesBySubjectId(queryDto);

            return Ok(studentGrades.Data);
        }

        [Authorize(Roles = "Teacher,Admin")]
        [HttpPost]
        public async Task<ActionResult<NewGradeDto>> NewGrade(NewGradeDto newGrade)
        {
            var response = await _gradesService.AddGradeAsync(newGrade);

            if (!response.IsSuccess) return BadRequest(response.Error.Message);

            return Ok();
        }

        [Authorize(Roles = "Teacher,Admin")]
        [HttpPatch]
        public async Task<ActionResult<Grades>> EditGrade(NewGradeDto newGrade)
        {
            var response = await _gradesService.EditGradeAsync(newGrade);

            if (response.IsSuccess) return Ok();

            return response.Error.Code switch
            {
                "grade.not.found" => NotFound(response.Error.Message),

                _ => BadRequest(response.Error.Message)
            };
        }
    }
}
