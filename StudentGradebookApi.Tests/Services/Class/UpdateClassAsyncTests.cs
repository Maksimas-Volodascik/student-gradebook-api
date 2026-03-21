using Moq;
using StudentGradebookApi.DTOs.Classes;
using StudentGradebookApi.Models;
using StudentGradebookApi.Repositories.ClassesRepository;
using StudentGradebookApi.Services.ClassesServices;
using StudentGradebookApi.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGradebookApi.Tests.Services.Class
{
    public class UpdateClassAsyncTests
    {
        private readonly Mock<IClassesRepository> _classesRepositoryMock;
        private readonly ClassesServices _classesService;

        public UpdateClassAsyncTests()
        {
            _classesRepositoryMock = new Mock<IClassesRepository>();

            _classesService = new ClassesServices(_classesRepositoryMock.Object);
        }

        [Fact] 
        public async Task UpdateClassAsync_ValidData_ReturnsSuccess()
        {
            int classId = 1;
            var dto = new NewClassDto
            {
                room = 101,
                academicYear = "2025-2026"
            };
            var existingClass = new Classes 
            { 
                Room = 200, 
                AcademicYear = "2024-2025" 
            };

            _classesRepositoryMock.Setup(s => s.GetByIdAsync(classId))
                .ReturnsAsync(existingClass);
            _classesRepositoryMock.Setup(r => r.Update(It.IsAny<Classes>()));
            _classesRepositoryMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = await _classesService.UpdateClassAsync(classId, dto);

            Assert.True(result.IsSuccess);
            Assert.Equal(dto.room, existingClass.Room);
            Assert.Equal(dto.academicYear, existingClass.AcademicYear);
            _classesRepositoryMock.Verify(r => r.Update(existingClass), Times.Once);
            _classesRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateClassAsync_InvalidAcademicYear_ReturnsFailure()
        {
            int classId = 1;
            var dto = new NewClassDto
            {
                room = 101,
                academicYear = "2025"
            };

            var result = await _classesService.UpdateClassAsync(classId, dto);

            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.ClassesErrors.InvalidClassesAcademicYear, result.Error);
            _classesRepositoryMock.Verify(r => r.Update(It.IsAny<Classes>()), Times.Never);
            _classesRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateClassAsync_InvalidRoomNumber_ReturnsFailure()
        {
            int classId = 1;
            var dto = new NewClassDto
            {
                room = -101,
                academicYear = "2025-2026"
            };

            var result = await _classesService.UpdateClassAsync(classId, dto);

            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.ClassesErrors.InvalidClassesRoom, result.Error);
            _classesRepositoryMock.Verify(r => r.Update(It.IsAny<Classes>()), Times.Never);
            _classesRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }
    }
}
