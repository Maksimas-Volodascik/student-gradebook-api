using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentGradebookApi.DTOs.Enrollments;
using StudentGradebookApi.DTOs.Grades;
using StudentGradebookApi.DTOs.SharedDto;
using StudentGradebookApi.Models;
using StudentGradebookApi.Services.EnrollmentsServices;
using StudentGradebookApi.Services.GradesServices;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StudentGradebookApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentServices _enrollmentServices;
        private readonly IGradesServices _gradesServices;
        public EnrollmentsController(IEnrollmentServices enrollmentServices, IGradesServices gradesServices) {
            _enrollmentServices = enrollmentServices;
            _gradesServices = gradesServices;
        }
        [Authorize(Roles = "Student,Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentEnrollments>>> GetStudentEnrollments([FromQuery] QueryDto queryDto)
        {
            int userId = Int32.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var response = await _enrollmentServices.GetStudentEnrollments(userId, queryDto);

            return Ok(response);
        }

        [Authorize(Roles = "Student,Admin")]
        [HttpPost("{classSubjectId}")]
        public async Task<ActionResult<Enrollments>> EnrollStudent(int classSubjectId)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)!.Value;
            await _enrollmentServices.EnrollStudent(classSubjectId, userEmail);
            return Ok();
        }

    }
}
