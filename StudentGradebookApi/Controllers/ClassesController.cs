using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentGradebookApi.DTOs.Classes;
using StudentGradebookApi.Models;
using StudentGradebookApi.Services.ClassesServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StudentGradebookApi.Controllers
{
    [Authorize]
    [Route("api/classes")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly IClassesServices _classesServices;
        public ClassesController(IClassesServices classesServices) {
            _classesServices = classesServices;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassSubjectsDto>>> GetAllClassesAsync([FromQuery] ClassesQueryDto classesQuery)
        {
            var response = await _classesServices.GetAllClassesAsync(classesQuery);
            return Ok(response.Data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Classes>> GetClassByIdAsync(int id)
        {
            var response = await _classesServices.GetClassByIdAsync(id);
            if (!response.IsSuccess) return NotFound(response.Error);
            
            return Ok(response.Data);
        }

        [Authorize(Roles = "Teacher,Admin")]
        [HttpPost]
        public async Task<ActionResult<NewClassDto>> AddClassAsync(NewClassDto classesContentsDTO)
        {
            var response = await _classesServices.AddClassAsync(classesContentsDTO);

            return Ok();
        }

        [Authorize(Roles = "Teacher,Admin")]
        [HttpPatch("{id}")]
        public async Task<ActionResult<NewClassDto>> UpdateClassAsync(int id, NewClassDto classesContentsDTO)
        {
            var response = await _classesServices.UpdateClassAsync(id, classesContentsDTO);

            if (response.IsSuccess) return Ok();

            return response.Error.Code switch
            {
                "class.not.found" => NotFound(response.Error.Message),

                _ => BadRequest(response.Error.Message)
            };
        }
    }
}
