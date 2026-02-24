using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using StudentGradebookApi.DTOs.Users;
using StudentGradebookApi.Models;
using StudentGradebookApi.Repositories.UsersRepository;
using StudentGradebookApi.Services.UserServices;
using StudentGradebookApi.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGradebookApi.Tests.Services.User
{
    public class UserLoginAsyncTests
    {
        private readonly Mock<IUsersRepository> _mockUserRepo;
        private readonly UserService _userService;

        public UserLoginAsyncTests() {
            _mockUserRepo = new Mock<IUsersRepository>();

            var inMemorySettings = new Dictionary<string, string>
            {
                { "AppSettings:Token", "very_long_secret_key_very_long_secret_key_very_long_secret_key_123456789" },
                { "AppSettings:Issuer", "TestIssuer" },
                { "AppSettings:Audience", "TestAudience" }
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _userService = new UserService(configuration, _mockUserRepo.Object);
        }

        public static class UserBuilder
        {
            public static LoginDTO Build()
            {
                return new LoginDTO
                { 
                    Email = "email@email.com",
                    Password = "password123",
                };
            }
        }

        [Fact]
        public async Task LoginAsync_ValidCredentials_ReturnsAccessToken()
        {
            // Arrange
            var loginDto = UserBuilder.Build();

            var user = new WebUsers
            {
                Id = 1,
                Email = loginDto.Email,
                Role = "demo"
            };

            user.PasswordHash = new PasswordHasher<WebUsers>()
                .HashPassword(user, loginDto.Password);

            _mockUserRepo.Setup(u => u.GetByEmailAsync(loginDto.Email))
                .ReturnsAsync(user);

            // Act
            var result = await _userService.LoginAsync(loginDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.False(string.IsNullOrEmpty(result.Data.AccessToken));

            _mockUserRepo.Verify(u => u.GetByEmailAsync(loginDto.Email), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_InvalidPassword_ReturnsUserErrors()
        {
            //Arrange
            var loginDto = UserBuilder.Build();
            var user = new WebUsers
            {
                Id = 1,
                Email = loginDto.Email,
                PasswordHash = new PasswordHasher<WebUsers>().HashPassword(null, "differentPassword")
            };

            _mockUserRepo.Setup(u => u.GetByEmailAsync(loginDto.Email))
                .ReturnsAsync(user);

            //Act
            var result = await _userService.LoginAsync(loginDto);

            //Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(result.Error, Errors.UserErrors.InvalidUserCredentials);
            _mockUserRepo.Verify(u => u.GetByEmailAsync(loginDto.Email), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_InvalidEmail_ReturnsUserErrors()
        {
            //Arrange
            var loginDto = UserBuilder.Build();

            _mockUserRepo.Setup(u => u.GetByEmailAsync(loginDto.Email))
                .ReturnsAsync((WebUsers)null);

            //Act
            var result = await _userService.LoginAsync(loginDto);

            //Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(result.Error, Errors.UserErrors.InvalidUserCredentials);
            _mockUserRepo.Verify(u => u.GetByEmailAsync(loginDto.Email), Times.Once);
        }
    }
}
