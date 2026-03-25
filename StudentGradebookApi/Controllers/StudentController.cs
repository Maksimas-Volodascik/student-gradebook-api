using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentGradebookApi.DTOs.Students;
using StudentGradebookApi.Models;
using StudentGradebookApi.Services.StudentServices;

namespace StudentGradebookApi.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;
        public StudentController(IStudentService studentService, IMapper mapper)
        {
            _studentService = studentService;
            _mapper = mapper;
        }

        [Authorize(Roles = "Student,Teacher,Admin")]
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<StudentList>>> GetStudents([FromQuery] StudentsQueryDto queryDto)
        {
            var response = await _studentService.GetAllStudentsAsync(queryDto);

            return Ok(response.Data);
        }

        [Authorize(Roles = "Teacher,Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentList>> GetStudent(int id)
        {
            var response = await _studentService.GetStudentByIdAsync(id);

            if (!response.IsSuccess) return NotFound(response.Error);

            StudentList student = _mapper.Map<StudentList>(response.Data);

            return Ok(student);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost()]
        public async Task<ActionResult> AddStudent(NewStudent studentData)
        {
            var response = await _studentService.AddStudentAsync(studentData);

            if(response.IsSuccess) return Ok();

            return response.Error.Code switch
            {
                "user.email.exists" => Conflict(response.Error.Message),
                _ => BadRequest(response.Error.Message)
            };
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}")]
        public async Task<ActionResult> EditStudent(EditStudent studentData, int id)
        {
            var response = await _studentService.EditStudentAsync(studentData, id);

            if (response.IsSuccess) return Ok();

            return response.Error.Code switch
            {
                "student.not.found" => NotFound(response.Error.Message),
                _ => BadRequest(response.Error.Message)
            };
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStudent(int id)
        {
            var response = await _studentService.DeleteStudentAsync(id);

            if (response.IsSuccess) return Ok();

            return response.Error.Code switch
            {
                "student.not.found" => NotFound(response.Error.Message),
                _ => BadRequest(response.Error.Message)
            };
        }
    }
}
