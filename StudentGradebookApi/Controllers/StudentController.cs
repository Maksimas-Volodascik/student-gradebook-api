using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StudentGradebookApi.DTOs.Students;
using StudentGradebookApi.Models;
using StudentGradebookApi.Services.StudentServices;

namespace StudentGradebookApi.Controllers
{
    [Route("api/student")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;
        public StudentController(IStudentService studentService, IMapper mapper)
        {
            _studentService = studentService;
            _mapper = mapper;
        }
        
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<StudentList>>> GetStudents([FromQuery] StudentsQueryDto queryDto)
        {
            var students = await _studentService.GetAllStudentsAsync(queryDto);
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentList>> GetStudent(int id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);

            if (!student.IsSuccess) return NotFound(student.Error);

            StudentList response = _mapper.Map<StudentList>(student.Data);

            return Ok(response);
        }

        [HttpPost()]
        public async Task<ActionResult> AddStudent(NewStudent studentData)
        {
            var response = await _studentService.AddStudentAsync(studentData);

            if(!response.IsSuccess) return BadRequest(response.Error);

            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> EditStudent(EditStudent studentData, int id)
        {
            var response = await _studentService.EditStudentAsync(studentData, id);

            if (!response.IsSuccess) return BadRequest(response.Error);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStudent(int id)
        {
            var response = await _studentService.DeleteStudentAsync(id);
            if(!response.IsSuccess)
            {
                return NotFound(response.Error);
            }
            return Ok();
        }
    }
}
