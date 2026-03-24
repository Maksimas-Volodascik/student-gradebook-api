using Castle.Core.Configuration;
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
    public class UserRegisterAsyncTests
    {
        private readonly Mock<IUsersRepository> _mockUserRepo;
        private readonly UserService _userService;

        public UserRegisterAsyncTests()
        {
            _mockUserRepo = new Mock<IUsersRepository>();
            _userService = new UserService(null, _mockUserRepo.Object);
        }

        [Fact]
        public async Task RegisterAsync_InvalidEmail_ReturnsEmailInvalid()
        {
            var newUser = new NewUserDto
            {
                Email = "not-an-email",
                Password = "Password123!",
                Role = "User"
            };

            var result = await _userService.RegisterAsync(newUser);

            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.UserErrors.EmailInvalid, result.Error);
        }

        [Fact]
        public async Task RegisterAsync_EmptyEmail_ReturnsEmailRequired()
        {
            var newUser = new NewUserDto
            {
                Email = "   ",
                Password = "Password123!",
                Role = "User"
            };

            var result = await _userService.RegisterAsync(newUser);

            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.UserErrors.EmailRequired, result.Error);
        }

        [Fact]
        public async Task RegisterAsync_EmailAlreadyExists_ReturnsEmailExists()
        {
            var newUser = new NewUserDto
            {
                Email = "test@example.com",
                Password = "Password123!",
                Role = "User"
            };

            
            _mockUserRepo.Setup(r => r.GetByEmailAsync(newUser.Email))
                         .ReturnsAsync(new WebUsers());

            var result = await _userService.RegisterAsync(newUser);

            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.UserErrors.EmailExists, result.Error);
        }

        [Fact]
        public async Task RegisterAsync_ValidNewUser_ReturnsSuccess()
        {
            var newUser = new NewUserDto
            {
                Email = "test@example.com",
                Password = "Password123!",
                Role = "User"
            };

            _mockUserRepo.Setup(r => r.GetByEmailAsync(newUser.Email))
                         .ReturnsAsync((WebUsers)null);

            _mockUserRepo.Setup(r => r.AddAsync(It.IsAny<WebUsers>()))
                         .Returns(Task.CompletedTask);

            _mockUserRepo.Setup(r => r.SaveChangesAsync())
                         .Returns(Task.CompletedTask);

            var result = await _userService.RegisterAsync(newUser);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(newUser.Email, result.Data.Email);
            Assert.Equal(newUser.Role, result.Data.Role);
            Assert.NotNull(result.Data.PasswordHash);


            _mockUserRepo.Verify(r => r.AddAsync(It.IsAny<WebUsers>()), Times.Once);
            _mockUserRepo.Verify(r => r.SaveChangesAsync());
        }
    }
}
