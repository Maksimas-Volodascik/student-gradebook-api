using Moq;
using StudentGradebookApi.DTOs.Subjects;
using StudentGradebookApi.Models;
using StudentGradebookApi.Repositories.SubjectsRepository;
using StudentGradebookApi.Services.SubjectsService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGradebookApi.Tests.Services.Subject
{
    public class UpdateSubjectAsyncTests
    {
        private readonly Mock<ISubjectsRepository> _mockSubjRepository;
        private readonly SubjectsService _subjectsService;

        public UpdateSubjectAsyncTests()
        {
            _mockSubjRepository = new Mock<ISubjectsRepository>();
            _subjectsService = new SubjectsService(_mockSubjRepository.Object);
        }

        [Fact]
        public async Task UpdateSubjectAsync_ValidData_ReturnsSuccessResult()
        {
            var subject = new Subjects();

            var dto = new SujectContentsDTO
            {
                subjectName = "Math",
                subjectCode = "Math101",
                description = "Description about math"
            };

            _mockSubjRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                   .ReturnsAsync(subject);

            var result = await _subjectsService.UpdateSubjectAsync(1, dto);

            Assert.True(result.IsSuccess);
            _mockSubjRepository.Verify(r => r.Update(subject), Times.Once);
            _mockSubjRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateSubjectAsync_InvalidData_ReturnsFailureResult()
        {
            var dto = new SujectContentsDTO
            {
                subjectName = "",
                subjectCode = "",
                description = "Description about math"
            };

            var result = await _subjectsService.UpdateSubjectAsync(1, dto);

            Assert.False(result.IsSuccess);
            _mockSubjRepository.Verify(r => r.Update(It.IsAny<Subjects>()), Times.Never);
        }

        [Fact]
        public async Task UpdateSubjectAsync_SubjectNotFound_ReturnsFailureResult()
        {
            var dto = new SujectContentsDTO
            {
                subjectName = "Math",
                subjectCode = "Math101",
                description = "Description about math"
            };

            _mockSubjRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                   .ReturnsAsync((Subjects)null);

            var result = await _subjectsService.UpdateSubjectAsync(1, dto);

            Assert.False(result.IsSuccess);
            _mockSubjRepository.Verify(r => r.Update(It.IsAny<Subjects>()), Times.Never);
        }
    }
}
