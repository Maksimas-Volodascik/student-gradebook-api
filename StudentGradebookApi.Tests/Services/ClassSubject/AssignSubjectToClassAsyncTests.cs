using Moq;
using StudentGradebookApi.DTOs.ClassSubjects;
using StudentGradebookApi.Models;
using StudentGradebookApi.Repositories.ClassSubjectsRepository;
using StudentGradebookApi.Services.ClassesServices;
using StudentGradebookApi.Services.SubjectClassServices;
using StudentGradebookApi.Services.SubjectsService;
using StudentGradebookApi.Services.TeacherServices;
using StudentGradebookApi.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentGradebookApi.Tests.Services.ClassSubject
{
    public class AssignSubjectToClassAsyncTests
    {
        private readonly Mock<IClassSubjectsRepository> _classSubjectsRepositoryMock;
        private readonly Mock<ITeacherService> _teacherService;
        private readonly Mock<ISubjectsService> _subjectsService;
        private readonly Mock<IClassesServices> _classesService;
        private readonly ClassSubjectsService _classSubjectsService;

        public AssignSubjectToClassAsyncTests()
        {
            _classSubjectsRepositoryMock = new Mock<IClassSubjectsRepository>();
            _teacherService = new Mock<ITeacherService>();
            _subjectsService = new Mock<ISubjectsService>();
            _classesService = new Mock<IClassesServices>();

            _classSubjectsService = new ClassSubjectsService(_classSubjectsRepositoryMock.Object, _teacherService.Object, _subjectsService.Object, _classesService.Object);
        }

        [Fact]
        public async Task AssignSubjectToClassAsync_ValidData_ReturnsSuccessResult()
        {
            var subject = new Subjects{ Id = 1 };
            var classes = new Classes{ Id = 1 };
            var dto = new CombineClassSubjectDto
            {
                SubjectId = subject.Id,
                ClassId = classes.Id
            };

            _subjectsService.Setup(s => s.GetSubjectByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(Result<Subjects>.Success(subject));

            _classesService.Setup(s => s.GetClassByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(Result<Classes>.Success(classes));

            var result = await _classSubjectsService.AssignSubjectToClassAsync(dto);

            Assert.True(result.IsSuccess);
            _classSubjectsRepositoryMock.Verify(c => c.AddAsync(It.Is<ClassSubjects>(cs => 
            cs.SubjectId == subject.Id && cs.ClassId == classes.Id)), Times.Once);
            _classSubjectsRepositoryMock.Verify(c => c.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task AssignSubjectToClassAsync_SubjectNotFound_ReturnsFailure()
        {
            var dto = new CombineClassSubjectDto
            {
                SubjectId = 1,
                ClassId = 1
            };

            var error = Errors.SubjectErrors.SubjectNotFound;

            _subjectsService.Setup(s => s.GetSubjectByIdAsync(dto.SubjectId))
                .ReturnsAsync(Result<Subjects>.Failure(error));

            var result = await _classSubjectsService.AssignSubjectToClassAsync(dto);

            Assert.False(result.IsSuccess);
            Assert.Equal(error, result.Error);
            _classSubjectsRepositoryMock.Verify(r => r.AddAsync(It.IsAny<ClassSubjects>()),Times.Never);
            _classSubjectsRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task AssignSubjectToClassAsync_ClassNotFound_ReturnsFailure()
        {
            var dto = new CombineClassSubjectDto
            {
                SubjectId = 1,
                ClassId = 1
            };

            var subject = new Subjects { Id = 1 };
            var error = Errors.ClassesErrors.ClassNotFound;

            _subjectsService.Setup(s => s.GetSubjectByIdAsync(dto.SubjectId))
                .ReturnsAsync(Result<Subjects>.Success(subject));

            _classesService.Setup(s => s.GetClassByIdAsync(dto.ClassId))
                .ReturnsAsync(Result<Classes>.Failure(error));

            var result = await _classSubjectsService.AssignSubjectToClassAsync(dto);

            Assert.False(result.IsSuccess);
            Assert.Equal(error, result.Error);
            _classSubjectsRepositoryMock.Verify(r => r.AddAsync(It.IsAny<ClassSubjects>()), Times.Never);
            _classSubjectsRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }
    }
}
