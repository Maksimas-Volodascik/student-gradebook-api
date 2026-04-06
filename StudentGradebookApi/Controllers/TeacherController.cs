using Asp.Versioning;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentGradebookApi.DTOs.Teachers;
using StudentGradebookApi.Models;
using StudentGradebookApi.Services.TeacherServices;

namespace StudentGradebookApi.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpGet]
        public async Task<ActionResult<List<TeacherDto>>> GetTeachers([FromQuery] TeachersQueryDto queryDto)
        {
            var response = await _teacherService.GetAllTeachersAsync(queryDto);

            return Ok(response.Data);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> AddNewTeacher(TeacherRequestDTO newTeacher)
        {
            var response = await _teacherService.AddTeacherAsync(newTeacher);

            if (!response.IsSuccess) return BadRequest(response.Error.Message);

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}")]
        public async Task<ActionResult> EditTeacher(int id, TeacherRequestDTO teacher)
        {
            var response = await _teacherService.EditTeacherAsync(id, teacher);

            if (response.IsSuccess) return Ok();

            return response.Error.Code switch
            {
                "teacher.not.found" => NotFound(response.Error.Message),
                _ => BadRequest(response.Error.Message)
            };
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTeacher(int id)
        {
            var response = await _teacherService.DeleteTeacherAsync(id);

            if (response.IsSuccess) return Ok();

            return response.Error.Code switch
            {
                "teacher.not.found" => NotFound(response.Error.Message),
                _ => BadRequest(response.Error.Message)
            };
        }
    }
}
