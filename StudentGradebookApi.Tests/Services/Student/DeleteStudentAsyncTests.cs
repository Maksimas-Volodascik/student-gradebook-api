using Moq;
using StudentGradebookApi.Models;
using StudentGradebookApi.Repositories.StudentsRepository;
using StudentGradebookApi.Services.StudentServices;
using StudentGradebookApi.Services.UserServices;
using StudentGradebookApi.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGradebookApi.Tests.Services.Student
{
    public class DeleteStudentAsyncTests
    {
        private readonly Mock<IStudentsRepository> _mockStudRepository;
        private readonly Mock<IUserService> _mockUserService;
        private readonly StudentService _studentService;

        public DeleteStudentAsyncTests()
        {
            _mockStudRepository = new Mock<IStudentsRepository>();
            _mockUserService = new Mock<IUserService>();
            _studentService = new StudentService(_mockUserService.Object, _mockStudRepository.Object);
        }

        [Fact]
        public async Task DeleteStudentAsync_StudentExists_DeletesUserAndReturnsSuccess()
        {
            int studentId = 1;
            int userId = 10;

            var student = new Students
            {
                Id = studentId,
                UserID = userId
            };

            _mockStudRepository.Setup(x => x.GetByIdAsync(studentId))
                .ReturnsAsync(student);

            _mockUserService.Setup(x => x.DeleteUserAsync(userId))
                .ReturnsAsync(Result.Success);

            var result = await _studentService.DeleteStudentAsync(studentId);

            Assert.True(result.IsSuccess);
            _mockUserService.Verify(x => x.DeleteUserAsync(userId), Times.Once);
        }

        [Fact]
        public async Task DeleteStudentAsync_StudentNotFound_ReturnsFailureResult()
        {
            int studentId = 1;

            _mockStudRepository.Setup(x => x.GetByIdAsync(studentId))
                .ReturnsAsync((Students?)null);

            var result = await _studentService.DeleteStudentAsync(studentId);

            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.StudentErrors.StudentNotFound, result.Error);
            _mockUserService.Verify(x => x.DeleteUserAsync(It.IsAny<int>()), Times.Never);
        }
    }
}
