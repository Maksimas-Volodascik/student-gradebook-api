using StudentGradebookApi.DTOs.Users;
using StudentGradebookApi.Models;
using StudentGradebookApi.Shared;

namespace StudentGradebookApi.Services.UserServices
{
    public interface IUserService
    {
        Task<Result<WebUsers>> RegisterAsync(RegisterDto request, string role);
        Task<Result<TokenResponse>> LoginAsync(LoginDto request);
        Task<TokenResponse> RefreshTokensAsync(RefreshTokenRequest request);
        Task<Result<WebUsers>> GetUserByIdAsync(int id);
        Task<Result> DeleteUserAsync(int id);
    }
}
