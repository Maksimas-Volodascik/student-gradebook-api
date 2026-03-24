using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using StudentGradebookApi.DTOs.Students;
using StudentGradebookApi.DTOs.SubjectClass;
using StudentGradebookApi.DTOs.Users;
using StudentGradebookApi.Models;
using StudentGradebookApi.Services.SubjectClassServices;
using StudentGradebookApi.Services.UserServices;

namespace StudentGradebookApi.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        //Register user
        [Authorize(Roles = "Admin")]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser (RegisterDto newUser)
        {
            var result = await _userService.RegisterAsync(newUser, "Demo");

            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Error);
        }
        //Login user
        [HttpPost("login")]
        public async Task<ActionResult<TokenResponse>> LoginUser (LoginDto loginDto)
        {
            var response = await _userService.LoginAsync(loginDto);
            if (!response.IsSuccess)
                return BadRequest(response.Error);
            return Ok(response.Data);
        }
        [Authorize]
        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponse>> RefreshToken(RefreshTokenRequest request)
        {
            var result = await _userService.RefreshTokensAsync(request);
            if (result is null || result.AccessToken is null || result.RefreshToken is null)
                return Unauthorized("Invalid refresh token.");
            return Ok(result);
        }
        /*[Authorize(Roles ="Admin")]
          [HttpGet("admin-only")]
          public IActionResult AdminOnlyOnlyEndpoint()
          {
              return Ok("You are admin!");
          }*/
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser (int id)
        {
            await _userService.DeleteUserAsync(id);
            return Ok();
        }

    }
}
