using Moq;
using StudentGradebookApi.DTOs.Students;
using StudentGradebookApi.DTOs.Subjects;
using StudentGradebookApi.DTOs.Teachers;
using StudentGradebookApi.DTOs.Users;
using StudentGradebookApi.Models;
using StudentGradebookApi.Repositories.StudentsRepository;
using StudentGradebookApi.Repositories.SubjectsRepository;
using StudentGradebookApi.Services.StudentServices;
using StudentGradebookApi.Services.SubjectsService;
using StudentGradebookApi.Services.UserServices;
using StudentGradebookApi.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGradebookApi.Tests.Services.Student
{
    public class AddStudentAsyncTests
    {
        private readonly Mock<IStudentsRepository> _mockStudRepository;
        private readonly Mock<IUserService> _mockUserService;
        private readonly StudentService _studentService;

        public AddStudentAsyncTests()
        {
            _mockStudRepository = new Mock<IStudentsRepository>();
            _mockUserService = new Mock<IUserService>();
            _studentService = new StudentService(_mockUserService.Object, _mockStudRepository.Object);
        }

        public static class NewStudentDTOBuilder
        {
            public static NewStudent Build()
            {
                return new NewStudent
                {
                    Email = "john.doe@example.com",
                    Password = "P@ssw0rd123!",
                    FirstName = "John",
                    LastName = "Doe",
                    DateOfBirth = new DateTime(2002, 5, 14)
                };
            }
        }

        [Fact]
        public async Task AddStudentAsync_ValidData_ReturnsSuccessResult()
        {
            var studentData = NewStudentDTOBuilder.Build();
            var webUser = new WebUsers
            {
                Id = 1,
                Email = studentData.Email
            };

            _mockUserService.Setup(u => u.RegisterAsync(It.IsAny<NewUserDto>()))
                .ReturnsAsync(Result<WebUsers>.Success(webUser));

            _mockStudRepository.Setup(s => s.AddAsync(It.IsAny<Students>()))
                .Returns(Task.CompletedTask);

            _mockStudRepository.Setup(s => s.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            var result = await _studentService.AddStudentAsync(studentData);

            Assert.True(result.IsSuccess);
            Assert.Equal(studentData.Email, webUser.Email);
            _mockUserService.Verify(u => u.RegisterAsync(It.IsAny<NewUserDto>()), Times.Once);
            _mockStudRepository.Verify(r => r.AddAsync(It.IsAny<Students>()), Times.Once);
            _mockStudRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task AddStudentAsync_NullStudentData_ReturnsFailureResult()
        {
            var result = await _studentService.AddStudentAsync(null);

            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.StudentErrors.StudentDataNull, result.Error);
            _mockUserService.Verify(x => x.RegisterAsync(It.IsAny<NewUserDto>()), Times.Never);
            _mockStudRepository.Verify(x => x.AddAsync(It.IsAny<Students>()), Times.Never);
            _mockStudRepository.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task AddStudentAsync_UserRegistrationFails_ReturnsFailureResult()
        {
            var studentData = NewStudentDTOBuilder.Build();

            var expectedError = Errors.UserErrors.EmailExists;

            _mockUserService.Setup(x => x.RegisterAsync(It.IsAny<NewUserDto>()))
                .ReturnsAsync(Result<WebUsers>.Failure(expectedError));

            var result = await _studentService.AddStudentAsync(studentData);

            Assert.False(result.IsSuccess);
            Assert.Equal(expectedError, result.Error);
            _mockStudRepository.Verify(x => x.AddAsync(It.IsAny<Students>()), Times.Never);
            _mockStudRepository.Verify(x => x.SaveChangesAsync(), Times.Never);
        }
    }
}
