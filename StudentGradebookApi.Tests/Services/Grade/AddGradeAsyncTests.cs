using Moq;
using StudentGradebookApi.DTOs.Grades;
using StudentGradebookApi.DTOs.Students;
using StudentGradebookApi.Models;
using StudentGradebookApi.Repositories.GradesRepository;
using StudentGradebookApi.Services.GradesServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGradebookApi.Tests.Services.Grade
{
    public class AddGradeAsyncTests
    {
        private readonly Mock<IGradesRepository> _mockGradesRepository;
        private readonly GradesServices _gradesService;

        public AddGradeAsyncTests()
        {
            _mockGradesRepository = new Mock<IGradesRepository>();
            _gradesService = new GradesServices(_mockGradesRepository.Object);
        }

        [Fact]
        public async Task AddGradeAsync_ValidData_AddsGradeAndReturnsSuccess()
        {
            var dto = new NewGradeDTO
            {
                gradingDate = DateTime.UtcNow,
                gradeType = "Exam",
                score = 8,
                enrollmentId = 1
            };

            _mockGradesRepository.Setup(g => g.AddAsync(It.IsAny<Grades>()))
                .Returns(Task.CompletedTask);

            _mockGradesRepository.Setup(g => g.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            var result = await _gradesService.AddGradeAsync(dto);

            Assert.True(result.IsSuccess);
            _mockGradesRepository.Verify(x => x.AddAsync(It.IsAny<Grades>()), Times.Once);
            _mockGradesRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task AddGradeAsync_InvalidScore_ReturnsFailure()
        {
            var dto = new NewGradeDTO
            {
                gradingDate = DateTime.UtcNow,
                gradeType = "Exam",
                score = 11,
                enrollmentId = 1
            };

            var result = await _gradesService.AddGradeAsync(dto);

            Assert.False(result.IsSuccess);
            Assert.NotNull(result.Error);
            _mockGradesRepository.Verify(r => r.AddAsync(It.IsAny<Grades>()), Times.Never);
            _mockGradesRepository.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task AddGradeAsync_InvalidEnrollmentId_ReturnsFailure()
        {
            var dto = new NewGradeDTO
            {
                gradingDate = DateTime.UtcNow,
                gradeType = "Exam",
                score = 8,
                enrollmentId = 0
            };

            var result = await _gradesService.AddGradeAsync(dto);

            Assert.False(result.IsSuccess);
            Assert.NotNull(result.Error);
            _mockGradesRepository.Verify(r => r.AddAsync(It.IsAny<Grades>()), Times.Never);
            _mockGradesRepository.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task AddGradeAsync_EmptyGradeType_ReturnsFailure()
        {
            var dto = new NewGradeDTO
            {
                gradingDate = DateTime.UtcNow,
                gradeType = "",
                score = 8,
                enrollmentId = 1
            };

            var result = await _gradesService.AddGradeAsync(dto);

            Assert.False(result.IsSuccess);
            Assert.NotNull(result.Error);
            _mockGradesRepository.Verify(r => r.AddAsync(It.IsAny<Grades>()), Times.Never);
            _mockGradesRepository.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task AddGradeAsync_DefaultGradingDate_ReturnsFailure()
        {
            var dto = new NewGradeDTO
            {
                gradingDate = default(DateTime),
                gradeType = "Exam",
                score = 8,
                enrollmentId = 1
            };

            var result = await _gradesService.AddGradeAsync(dto);

            Assert.False(result.IsSuccess);
            Assert.NotNull(result.Error);
            _mockGradesRepository.Verify(r => r.AddAsync(It.IsAny<Grades>()), Times.Never);
            _mockGradesRepository.Verify(r => r.SaveChangesAsync(), Times.Never);
        }
    }
}
