using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using NuGet.Protocol;
using StackExchange.Redis;
using StudentGradebookApi.DTOs.Grades;
using StudentGradebookApi.Models;
using StudentGradebookApi.Repositories.GradesRepository;
using StudentGradebookApi.Services.GradesServices;
using StudentGradebookApi.Shared;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StudentGradebookApi.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class GradesController : ControllerBase
    {
        private readonly IGradesServices _gradesService;
        private readonly StackExchange.Redis.IDatabase _redis;

        public GradesController(IGradesServices gradesServices, IConnectionMultiplexer muxer) { 
            _gradesService = gradesServices;
            _redis = muxer.GetDatabase();
        }

        [Authorize(Roles = "Student,Teacher,Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentGradesBySubjectDto>>> GetStudentGrades([FromQuery] GradesQueryDto queryDto)
        {
            var serializedQuery = JsonSerializer.Serialize(queryDto);
            var queryHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(serializedQuery)));

            var keyName = $"query:{queryHash}";
            string json = await _redis.StringGetAsync(keyName);

            if (string.IsNullOrEmpty(json))
            {
                var studentData = await _gradesService.GetStudentGradesBySubjectId(queryDto);
                json = JsonSerializer.Serialize(studentData.Data);

                var setTask = _redis.StringSetAsync(keyName, json);
                var expireTask = _redis.KeyExpireAsync(keyName, TimeSpan.FromSeconds(3600));

                await Task.WhenAll(setTask, expireTask); //await both tasks in Parallel
            }

            var studentGrades = JsonSerializer.Deserialize<IEnumerable<StudentGradesBySubjectDto>>(json);
            //var studentGrades = await _gradesService.GetStudentGradesBySubjectId(queryDto);

            return Ok(studentGrades);
        }

        [Authorize(Roles = "Teacher,Admin")]
        [HttpPost]
        public async Task<ActionResult<NewGradeDto>> NewGrade(NewGradeDto newGrade)
        {
            var response = await _gradesService.AddGradeAsync(newGrade);

            if (!response.IsSuccess) return BadRequest(response.Error.Message);

            return Ok();
        }

        [Authorize(Roles = "Teacher,Admin")]
        [HttpPatch]
        public async Task<ActionResult<Grades>> EditGrade(NewGradeDto newGrade)
        {
            var response = await _gradesService.EditGradeAsync(newGrade);

            if (response.IsSuccess) return Ok();

            return response.Error.Code switch
            {
                "grade.not.found" => NotFound(response.Error.Message),

                _ => BadRequest(response.Error.Message)
            };
        }
    }
}
