using Microsoft.AspNetCore.Mvc;
using StudentGradebookApi.DTOs.Grades;
using StudentGradebookApi.Models;
using StudentGradebookApi.Repositories.GradesRepository;
using StudentGradebookApi.Services.GradesServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StudentGradebookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradesController : ControllerBase
    {
        private readonly IGradesServices _gradesService;
        public GradesController(IGradesServices gradesServices) { 
            _gradesService = gradesServices;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentGradesBySubjectDTO>>> GetStudentGrades([FromQuery] GradesQueryDto queryDto)
        {
            var studentGrades = await _gradesService.GetStudentGradesBySubjectId(queryDto);
            return Ok(studentGrades);
        }

        [HttpPost]
        public async Task<ActionResult<NewGradeDTO>> NewGrade(NewGradeDTO newGrade)
        {
            await _gradesService.AddGradeAsync(newGrade);
            return Ok();
        }

        [HttpPatch]
        public async Task<ActionResult<Grades>> EditGrade(NewGradeDTO newGrade)
        {
            var response = await _gradesService.EditGradeAsync(newGrade);
            return Ok(response);
        }
    }
}
