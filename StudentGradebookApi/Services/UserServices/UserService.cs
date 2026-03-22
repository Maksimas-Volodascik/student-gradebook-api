using Humanizer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using StudentGradebookApi.DTOs.Users;
using StudentGradebookApi.Models;
using StudentGradebookApi.Repositories.Main;
using StudentGradebookApi.Repositories.UsersRepository;
using StudentGradebookApi.Shared;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace StudentGradebookApi.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IUsersRepository _userRepository;
        private readonly IConfiguration _configuration;
        public UserService(IConfiguration configuration, IUsersRepository userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
        }
        
        public async Task<Result<WebUsers>> RegisterAsync(RegisterDto newUser, string role)
        {
            if (string.IsNullOrWhiteSpace(newUser.Email))
                return Result<WebUsers>.Failure(Errors.UserErrors.EmailRequired);

            if (string.IsNullOrWhiteSpace(newUser.Password))
                return Result<WebUsers>.Failure(Errors.UserErrors.PasswordRequired);

            var emailValidator = new EmailAddressAttribute();
            if (!emailValidator.IsValid(newUser.Email))
                return Result<WebUsers>.Failure(Errors.UserErrors.EmailInvalid);

            if (await _userRepository.GetByEmailAsync(newUser.Email) != null)
                return Result<WebUsers>.Failure(Errors.UserErrors.EmailExists);

            WebUsers user = new WebUsers();
            var passwordHasher = new PasswordHasher<WebUsers>();
            var hashedPassword = passwordHasher.HashPassword(user, newUser.Password);
            user.Email = newUser.Email;
            user.PasswordHash = hashedPassword;
            user.Role = role == null ? "demo" : role;

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return Result<WebUsers>.Success(user);
        }

        public async Task<Result<WebUsers>> GetUserByIdAsync(int id)
        {
            var result = await _userRepository.GetByIdAsync(id);
            if (result == null) return Result<WebUsers>.Failure(Errors.UserErrors.UserNotFound);

            return Result<WebUsers>.Success(result);
        }

        public async Task<Result> DeleteUserAsync(int id)
        {
            WebUsers? user = await _userRepository.GetByIdAsync(id);
            if (user == null) return Result.Failure(Errors.UserErrors.UserNotFound);

            _userRepository.Delete(user);
            await _userRepository.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<TokenResponse>> LoginAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);
            if (user is null || new PasswordHasher<WebUsers>().VerifyHashedPassword(user, user.PasswordHash, loginDto.Password) == PasswordVerificationResult.Failed)
            {
                return Result<TokenResponse>.Failure(Errors.UserErrors.InvalidUserCredentials);
            }

            TokenResponse token = await CreateTokenResponse(user);

            return Result<TokenResponse>.Success(token);
        }

        private async Task<TokenResponse> CreateTokenResponse(WebUsers user)
        {
            return new TokenResponse
            {
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsyc(user)
            };
        }

        private async Task<WebUsers?> ValidateRefreshTokenAsync(int userId, string refreshToken)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }
            return user;
        }

        public async Task<TokenResponse> RefreshTokensAsync(RefreshTokenRequest request)
        {
            var user = await ValidateRefreshTokenAsync(request.UserID, request.RefreshToken);
            if (user is null)
                return null;
            return await CreateTokenResponse(user);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<string> GenerateAndSaveRefreshTokenAsyc(WebUsers user)
        {
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(14);
            await _userRepository.SaveChangesAsync();
            return refreshToken;
        }

        private string CreateToken(WebUsers user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:Token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("AppSettings:Issuer"),
                audience: _configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
