using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentGradebookApi.DTOs.ClassSubjects;
using StudentGradebookApi.DTOs.SubjectClass;
using StudentGradebookApi.Models;
using StudentGradebookApi.Services.SubjectClassServices;

namespace StudentGradebookApi.Controllers
{
    [Authorize]
    [Route("api/class-subjects")]
    [ApiController]
    public class ClassSubjectsController : ControllerBase
    {
        private readonly IClassSubjectsService _classSubjectsService;
        public ClassSubjectsController(IClassSubjectsService classSubjectsService)
        {
            _classSubjectsService = classSubjectsService;
        }

        [Authorize(Roles = "Teacher,Admin")]
        [HttpPatch]
        public async Task<ActionResult<CombineClassSubjectDto>> EditClassSubjectTeacher(CombineClassSubjectDto combineClassSubjectDTO)
        {
            var response = await _classSubjectsService.AssignSubjectToClassAsync(combineClassSubjectDTO);
            return Ok(response);
        }

        [Authorize(Roles = "Teacher,Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteClassSubject(int id)
        {
            var response = await _classSubjectsService.RemoveSubjectClassAsync(id);
            return Ok();
        }
    }
}
