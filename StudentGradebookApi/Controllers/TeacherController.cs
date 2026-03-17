using Microsoft.AspNetCore.Mvc;
using StudentGradebookApi.DTOs.Teachers;
using StudentGradebookApi.Models;
using StudentGradebookApi.Services.TeacherServices;

namespace StudentGradebookApi.Controllers
{
    [Route("api/teacher")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpGet]
        public async Task<ActionResult<List<TeacherDTO>>> GetTeachers([FromQuery] TeachersQueryDto queryDto)
        {
            var response = await _teacherService.GetAllTeachersAsync(queryDto);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult> AddNewTeacher(TeacherRequestDTO newTeacher)
        {
            await _teacherService.AddTeacherAsync(newTeacher);
            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> EditTeacher(int id, TeacherRequestDTO teacher)
        {
            await _teacherService.EditTeacherAsync(id, teacher);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTeacher(int id)
        {
            var response = await _teacherService.DeleteTeacherAsync(id);
            if (response == null)
            {
                return BadRequest(new { message = "User not found" });
            }
            return Ok();
        }
    }
}
