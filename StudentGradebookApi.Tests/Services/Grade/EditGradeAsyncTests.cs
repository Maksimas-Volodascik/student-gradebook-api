using Moq;
using StudentGradebookApi.DTOs.Grades;
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
    public class EditGradeAsyncTests
    {
        private readonly Mock<IGradesRepository> _mockGradesRepository;
        private readonly GradesServices _gradesService;

        public EditGradeAsyncTests()
        {
            _mockGradesRepository = new Mock<IGradesRepository>();
            _gradesService = new GradesServices(_mockGradesRepository.Object);
        }

        [Fact]
        public async Task EditGradeAsync_ValidData_UpdatesGradeAndReturnsSuccess()
        {
            var dto = new NewGradeDTO
            {
                gradingDate = DateTime.UtcNow.Date,
                gradeType = "exam",
                score = 8,
                enrollmentId = 1
            };

            var existingGrade = new Grades
            {
                GradingDate = dto.gradingDate,
                Grade_Type = "test",
                Score = 9,
                EnrollmentId = dto.enrollmentId
            };

            _mockGradesRepository.Setup(r => r.GetGradeByDateAndEnrollmentId(dto.gradingDate.Date, dto.enrollmentId))
                .ReturnsAsync(existingGrade);

            _mockGradesRepository.Setup(r => r.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            var result = await _gradesService.EditGradeAsync(dto);

            Assert.True(result.IsSuccess);
            Assert.Equal(dto.gradeType, existingGrade.Grade_Type);
            Assert.Equal(dto.score, existingGrade.Score);
            _mockGradesRepository.Verify(r => r.Update(existingGrade), Times.Once);
            _mockGradesRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task EditGradeAsync_InvalidData_ReturnsFailure_AndDoesNotCallRepository()
        {
            var dto = new NewGradeDTO
            {
                gradingDate = default,
                gradeType = "exam",
                score = 90,
                enrollmentId = 1
            };

            var result = await _gradesService.EditGradeAsync(dto);

            Assert.False(result.IsSuccess);
            _mockGradesRepository.Verify(r => r.GetGradeByDateAndEnrollmentId(It.IsAny<DateTime>(), It.IsAny<int>()),Times.Never);
            _mockGradesRepository.Verify(r => r.Update(It.IsAny<Grades>()), Times.Never);
            _mockGradesRepository.Verify(r => r.SaveChangesAsync(), Times.Never);
        }
    }
}
