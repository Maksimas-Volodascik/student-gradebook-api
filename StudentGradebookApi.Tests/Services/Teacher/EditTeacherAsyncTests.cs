using Moq;
using StudentGradebookApi.DTOs.Teachers;
using StudentGradebookApi.Models;
using StudentGradebookApi.Repositories.TeachersRepository;
using StudentGradebookApi.Services.SubjectClassServices;
using StudentGradebookApi.Services.TeacherServices;
using StudentGradebookApi.Services.UserServices;
using StudentGradebookApi.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGradebookApi.Tests.Services.Teacher
{
    public class EditTeacherAsyncTests
    {
        private readonly TeacherService _teacherService;
        private readonly Mock<ITeachersRepository> _teacherRepMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IClassSubjectsService> _classSubjMock;

        public EditTeacherAsyncTests()
        {
            _teacherRepMock = new Mock<ITeachersRepository>();
            _classSubjMock = new Mock<IClassSubjectsService>();
            _userServiceMock = new Mock<IUserService>();

            _teacherService = new TeacherService(_teacherRepMock.Object, _userServiceMock.Object, _classSubjMock.Object);
        }
        public static class TeacherDTOBuilder
        {
            public static TeacherRequestDTO Build()
            {
                return new TeacherRequestDTO
                {
                    Email = "email@email.com",
                    Password = "password123",
                    FirstName = "Jim",
                    LastName = "Jimmy",
                    ClassSubjectId = 1
                };
            }
        }

        [Fact]
        public async Task EditTeacherAsync_ValidData_ReturnsSuccessResult()
        {
            //Arrange
            var teacherDTO = TeacherDTOBuilder.Build();

            var oldTeacherData = new Teachers
            {
                Id = 1,
                FirstName = "oldFirstName",
                LastName = "oldLastName"
            };

            _teacherRepMock.Setup(t => t.GetByIdAsync(1))
                .ReturnsAsync(oldTeacherData);

            _classSubjMock.Setup(c => c.EditSubjectClassTeacher(teacherDTO.ClassSubjectId,1))
                .ReturnsAsync(Result.Success);

            Teachers updatedTeacher = null;
            _teacherRepMock.Setup(t => t.Update(It.IsAny<Teachers>()))
                .Callback<Teachers>(t => updatedTeacher = t);

            _teacherRepMock.Setup(t => t.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            //Act
            var result = await _teacherService.EditTeacherAsync(1, teacherDTO);
            
            //Assert
            Assert.True(result.IsSuccess);
            Assert.Same(oldTeacherData, updatedTeacher);
            Assert.Equal(teacherDTO.FirstName, updatedTeacher.FirstName);
            Assert.Equal(teacherDTO.LastName, updatedTeacher.LastName);
            _teacherRepMock.Verify(t => t.GetByIdAsync(1), Times.Once);
            _classSubjMock.Verify(c => c.EditSubjectClassTeacher(teacherDTO.ClassSubjectId, 1), Times.Once);
            _teacherRepMock.Verify(t => t.Update(It.IsAny<Teachers>()), Times.Once);
            _teacherRepMock.Verify(t => t.SaveChangesAsync(), Times.Once);
            
        }

        [Fact]
        public async Task EditTeacherAsync_InvalidTeacherData_ReturnsFailureResult()
        {
            // Arrange
            var invalidTeacherDTO = new TeacherRequestDTO
            {
                Email = "",
                Password = "pwd",
                FirstName = "",
                LastName = "",
                ClassSubjectId = 1
            };

            // Act
            var result = await _teacherService.EditTeacherAsync(1, invalidTeacherDTO);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.NotNull(result.Error);
            _teacherRepMock.VerifyNoOtherCalls();
            _classSubjMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task EditTeacherAsync_TeacherNotFound_ReturnsFailureResult()
        {
            // Arrange
            var teacherDTO = TeacherDTOBuilder.Build();

            _teacherRepMock.Setup(t => t.GetByIdAsync(1))
                .ReturnsAsync((Teachers)null); 

            // Act
            var result = await _teacherService.EditTeacherAsync(1, teacherDTO);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.TeacherErrors.TeacherNotFound, result.Error);

            _teacherRepMock.Verify(t => t.GetByIdAsync(1), Times.Once);
            _teacherRepMock.VerifyNoOtherCalls();
            _classSubjMock.VerifyNoOtherCalls();
        }
    }
}
