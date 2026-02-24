using Microsoft.AspNetCore.Identity;
using Moq;
using StudentGradebookApi.DTOs.Teachers;
using StudentGradebookApi.DTOs.Users;
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
    public class AddTeacherAsyncTests
    {
        private readonly TeacherService _teacherService;
        private readonly Mock<ITeachersRepository> _teacherRepMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IClassSubjectsService> _classSubjMock;

        public AddTeacherAsyncTests() { 
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
        public async Task AddTeacherAsync_ValidData_ReturnsSuccessResult()
        {
            //Arrange
            var teacherDTO = TeacherDTOBuilder.Build();

            var webUser = new WebUsers
            {
                Id = 1,
                Email = teacherDTO.Email
            };

            _userServiceMock.Setup(u => u.RegisterAsync(It.IsAny<NewUserDTO>()))
                .ReturnsAsync(Result<WebUsers>.Success(webUser));

            _teacherRepMock.Setup(t => t.AddAsync(It.IsAny<Teachers>()))
                .Returns(Task.CompletedTask);

            _teacherRepMock.Setup(t => t.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            _teacherRepMock.Setup(t => t.GetTeacherByEmail(teacherDTO.Email))
                .ReturnsAsync(new Teachers
                {
                    Id = 1,
                    FirstName = teacherDTO.FirstName,
                    LastName = teacherDTO.LastName,
                    UserID = webUser.Id,
                });

            _classSubjMock.Setup(c => c.EditSubjectClassTeacher(teacherDTO.ClassSubjectId, It.IsAny<int>()))
                .ReturnsAsync(Result.Success);

            //Act
            var result = await _teacherService.AddTeacherAsync(teacherDTO);

            //Assert
            Assert.True(result.IsSuccess);
            _userServiceMock.Verify(u => u.RegisterAsync(It.IsAny<NewUserDTO>()), Times.Once);
            _teacherRepMock.Verify(t => t.AddAsync(It.IsAny<Teachers>()), Times.Once);
            _teacherRepMock.Verify(t => t.SaveChangesAsync(), Times.Once);
            _teacherRepMock.Verify(t => t.GetTeacherByEmail(teacherDTO.Email), Times.Once);
            _classSubjMock.Verify(c => c.EditSubjectClassTeacher(teacherDTO.ClassSubjectId, It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task AddTeacherAsync_InvalidFirstName_ReturnsFailureResult()
        {
            //Arrange
            var teacherDTO = TeacherDTOBuilder.Build();
            teacherDTO.FirstName = "";

            //Act
            var result = await _teacherService.AddTeacherAsync(teacherDTO);

            //Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(result.Error, Errors.TeacherErrors.FirstNameMissing);

            _userServiceMock.Verify(u => u.RegisterAsync(It.IsAny<NewUserDTO>()), Times.Never);
            _teacherRepMock.Verify(t => t.AddAsync(It.IsAny<Teachers>()), Times.Never);
        }

        [Fact]
        public async Task AddTeacherAsync_InvalidLastName_ReturnsFailureResult()
        {
            //Arrange
            var teacherDTO = TeacherDTOBuilder.Build();
            teacherDTO.LastName = "";

            //Act
            var result = await _teacherService.AddTeacherAsync(teacherDTO);

            //Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(result.Error, Errors.TeacherErrors.LastNameMissing);

            _userServiceMock.Verify(u => u.RegisterAsync(It.IsAny<NewUserDTO>()), Times.Never);
            _teacherRepMock.Verify(t => t.AddAsync(It.IsAny<Teachers>()), Times.Never);
        }
    }
}
