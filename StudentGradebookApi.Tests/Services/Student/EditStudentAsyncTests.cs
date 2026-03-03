using Moq;
using StudentGradebookApi.DTOs.Students;
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
    public class EditStudentAsyncTests
    {
        private readonly Mock<IStudentsRepository> _mockStudRepository;
        private readonly Mock<IUserService> _mockUserService;
        private readonly StudentService _studentService;

        public EditStudentAsyncTests()
        {
            _mockStudRepository = new Mock<IStudentsRepository>();
            _mockUserService = new Mock<IUserService>();
            _studentService = new StudentService(_mockUserService.Object, _mockStudRepository.Object);
        }

        [Fact]
        public async Task EditStudentAsync_ValidData_UpdatesStudentAndReturnsSuccess()
        {
            int studentId = 1;
            var studentData = new EditStudent
            {
                FirstName = "Jane",
                LastName = "Smith",
                DateOfBirth = new DateTime(2003, 6, 20)
            };

            var existingStudent = new Students
            {
                Id = studentId,
                FirstName = "OldFirstName",
                LastName = "OldLastName",
                DateOfBirth = new DateTime(2000, 1, 1)
            };

            _mockStudRepository.Setup(x => x.GetByIdAsync(studentId))
                .ReturnsAsync(existingStudent);

            _mockStudRepository.Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            var result = await _studentService.EditStudentAsync(studentData, studentId);

            Assert.True(result.IsSuccess);
            Assert.Equal("Jane", existingStudent.FirstName);
            Assert.Equal("Smith", existingStudent.LastName);
            Assert.Equal(new DateTime(2003, 6, 20), existingStudent.DateOfBirth);
            _mockStudRepository.Verify(x => x.Update(existingStudent), Times.Once);
            _mockStudRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task EditStudentAsync_StudentNotFound_ReturnsFailureResult()
        {
            var studentData = new EditStudent
            {
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(2002, 5, 14)
            };

            _mockStudRepository.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Students?)null);

            var result = await _studentService.EditStudentAsync(studentData, 1);

            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.StudentErrors.StudentNotFound, result.Error);
            _mockStudRepository.Verify(x => x.Update(It.IsAny<Students>()), Times.Never);
            _mockStudRepository.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task EditStudentAsync_NullStudentData_ReturnsFailureResult()
        {
            var result = await _studentService.EditStudentAsync(null, 1);

            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.StudentErrors.StudentDataNull, result.Error);

            _mockStudRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
            _mockStudRepository.Verify(x => x.Update(It.IsAny<Students>()), Times.Never);
            _mockStudRepository.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task EditStudentAsync_EmptyFirstName_ReturnsFailureResult()
        {
            var studentData = new EditStudent
            {
                FirstName = "", 
                LastName = "Doe",
                DateOfBirth = new DateTime(2002, 5, 14)
            };

            var result = await _studentService.EditStudentAsync(studentData, 1);

            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.StudentErrors.StudentFirstNameEmpty, result.Error);
            _mockStudRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
            _mockStudRepository.Verify(x => x.Update(It.IsAny<Students>()), Times.Never);
            _mockStudRepository.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task EditStudentAsync_EmptyLastName_ReturnsFailureResult()
        {
            var studentData = new EditStudent
            {
                FirstName = "John",
                LastName = "",
                DateOfBirth = new DateTime(2002, 5, 14)
            };

            var result = await _studentService.EditStudentAsync(studentData, 1);

            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.StudentErrors.StudentLastNameEmpty, result.Error);
            _mockStudRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
            _mockStudRepository.Verify(x => x.Update(It.IsAny<Students>()), Times.Never);
            _mockStudRepository.Verify(x => x.SaveChangesAsync(), Times.Never);
        }
    }
}
