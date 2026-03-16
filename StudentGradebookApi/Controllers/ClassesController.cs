using Microsoft.AspNetCore.Mvc;
using StudentGradebookApi.DTOs.Classes;
using StudentGradebookApi.Models;
using StudentGradebookApi.Services.ClassesServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StudentGradebookApi.Controllers
{
    [Route("api/classes")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly IClassesServices _classesServices;
        public ClassesController(IClassesServices classesServices) {
            _classesServices = classesServices;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Classes>>> GetAllClassesAsync([FromQuery] ClassesQueryDto classesQuery)
        {
            var response = await _classesServices.GetAllClassesAsync(classesQuery);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Classes>> GetClassByIdAsync(int id)
        {
            var response = await _classesServices.GetClassByIdAsync(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ClassesContentsDTO>> AddClassAsync(ClassesContentsDTO classesContentsDTO)
        {
            var response = await _classesServices.AddClassAsync(classesContentsDTO);
            return Ok(response);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<ClassesContentsDTO>> UpdateClassAsync(int id, ClassesContentsDTO classesContentsDTO)
        {
            var response = await _classesServices.UpdateClassAsync(id, classesContentsDTO);
            return Ok(response);
        }
    }
}
