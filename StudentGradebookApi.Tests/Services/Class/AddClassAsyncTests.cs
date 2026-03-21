using Moq;
using StudentGradebookApi.DTOs.Classes;
using StudentGradebookApi.Models;
using StudentGradebookApi.Repositories.ClassesRepository;
using StudentGradebookApi.Services.ClassesServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGradebookApi.Tests.Services.Class
{
    public class AddClassAsyncTests
    {
        private readonly Mock<IClassesRepository> _classesRepositoryMock;
        private readonly ClassesServices _classesService;

        public AddClassAsyncTests()
        {
            _classesRepositoryMock = new Mock<IClassesRepository>();

            _classesService = new ClassesServices(_classesRepositoryMock.Object);
        }

        [Fact] 
        public async Task AddClassAsync_ValidData_ReturnsSuccess()
        {
            var dto = new NewClassDto
            {
                room = 101,
                academicYear = "2025-2026"
            };

            _classesRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Classes>()))
                .Returns(Task.CompletedTask);

            _classesRepositoryMock.Setup(r => r.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            var result = await _classesService.AddClassAsync(dto);

            Assert.True(result.IsSuccess);
            _classesRepositoryMock.Verify(r => r.AddAsync(It.Is<Classes>(c => c.Room == dto.room && c.AcademicYear == dto.academicYear)), Times.Once);
            _classesRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
    }
}

