using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentGradebookApi.DTOs.Grades;
using StudentGradebookApi.Models;
using StudentGradebookApi.Repositories.GradesRepository;
using StudentGradebookApi.Services.GradesServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StudentGradebookApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
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
            return Ok(studentGrades);
        }

        [Authorize(Roles = "Teacher,Admin")]
        [HttpPost]
        public async Task<ActionResult<NewGradeDto>> NewGrade(NewGradeDto newGrade)
        {
            await _gradesService.AddGradeAsync(newGrade);
            return Ok();
        }

        [Authorize(Roles = "Teacher,Admin")]
        [HttpPatch]
        public async Task<ActionResult<Grades>> EditGrade(NewGradeDto newGrade)
        {
            var response = await _gradesService.EditGradeAsync(newGrade);
            return Ok(response);
        }
    }
}
