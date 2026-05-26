using AutoMapper;
using Exam.App.Domain;
using Exam.App.Domain.Repositories;
using Exam.App.Infrastructure.Database.Repositories;
using Exam.App.Services;
using Exam.App.Services.Dtos;
using Exam.App.Services.Exceptions;
using Exam.App.Services.Interfaces;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Tests.Services
{
    public class ExamReportServiceTest
    {

        private readonly Mock<IExamReportRepository> _reportRepository;
        private readonly Mock<IExaminationRepository> _examinationRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IVetRepository> _vetRepository;
        private readonly ExamReportService _reportService;

        public ExamReportServiceTest()
        {
            _examinationRepository = new Mock<IExaminationRepository>();
            _mapper = new Mock<IMapper>();
            _reportRepository = new Mock<IExamReportRepository>();
            _vetRepository = new Mock<IVetRepository>();
            _reportService = new ExamReportService
                (
                _reportRepository.Object,
                _examinationRepository.Object,
                _mapper.Object,
                _vetRepository.Object
                );
        }

        [Fact]
        public async Task UpdateReport_WhenExamDoesNotBelongToVet_ThrowsBadRequestException()
        {
            var report = new ExamReport{Id = 1, ExaminationId = 1};
            var exam = new Examination { Id = 1, VetId = 2 };// nije bitno, samo postoji
            var vet = new Vet
            {
                Id = 1,
                User = new ApplicationUser
                {
                    Name = "test",
                    Surname = "test",
                    UserName = "test"
                },
                Examinations = new List<Examination>() 
            };

            _reportRepository
                .Setup(r => r.GetReportById(1))
                .ReturnsAsync(report);

            _vetRepository
                .Setup(v => v.FindByUsername("test"))
                .ReturnsAsync(vet);

            _examinationRepository
                .Setup(e => e.GetById(1))
                .ReturnsAsync(exam);

            var dto = new UpdateReportDto
            {
                PatientWeight = 10,
                Anamnesis = "test"
            };

            await Should.ThrowAsync<BadRequestException>(
                () => _reportService.UpdateReport(dto, 1, "test")
            );
        }
        [Fact]
        public async Task UpdateReport_WhenExamBelongsToVet_Success()
        {
            var report = new ExamReport { Id = 1, ExaminationId = 1 };
            var exam = new Examination { Id = 1, VetId = 2 };
            var vet = new Vet
            {
                Id = 1,
                User = new ApplicationUser
                {
                    Name = "test",
                    Surname = "test",
                    UserName = "test"
                },
                Examinations = new List<Examination>()
            };
            vet.Examinations.Add(exam);
            _reportRepository
                .Setup(r => r.GetReportById(1))
                .ReturnsAsync(report);

            _vetRepository
                .Setup(v => v.FindByUsername("test"))
                .ReturnsAsync(vet);

            _examinationRepository
                .Setup(e => e.GetById(1))
                .ReturnsAsync(exam);

            var dto = new UpdateReportDto
            {
                PatientWeight = 10,
                Anamnesis = "test"
            };

            var result = await _reportService.UpdateReport(dto, 1, "test");
            Assert.NotNull(result);
            Assert.Equal(dto.PatientWeight, result.PatientWeight);
            Assert.Equal(dto.Anamnesis, result.Anamnesis);

            _reportRepository.Verify(r => r.UpdateReport(It.IsAny<ExamReport>()), Times.Once);
        }
        [Fact]
        public async Task UpdateReport_WhenExamStatusIsCancelled_ThrowBadRequestException()
        {
            var report = new ExamReport { Id = 1, ExaminationId = 1 };
            var exam = new Examination { Id = 1, Status = ExaminationStatus.Cancelled };
            
            _reportRepository
                .Setup(r => r.GetReportById(1))
                .ReturnsAsync(report);

            _examinationRepository
                .Setup(e => e.GetById(1))
                .ReturnsAsync(exam);

            var dto = new UpdateReportDto
            {
                PatientWeight = 10,
                Anamnesis = "test"
            };

            await Should.ThrowAsync<BadRequestException>(
                () => _reportService.UpdateReport(dto, 1, "test")
            );
        }
        [Fact]
        public async Task UpdateReport_WhenExamStatusIsNotCancelled_Success()
        {
            var report = new ExamReport { Id = 1, ExaminationId = 1 };
            var exam = new Examination { Id = 1, Status = ExaminationStatus.Active };
            var vet = new Vet
            {
                Id = 1,
                User = new ApplicationUser
                {
                    Name = "test",
                    Surname = "test",
                    UserName = "test"
                },
                Examinations = new List<Examination>()
            };
            vet.Examinations.Add(exam);
            _reportRepository
                .Setup(r => r.GetReportById(1))
                .ReturnsAsync(report);

            _vetRepository
                .Setup(v => v.FindByUsername("test"))
                .ReturnsAsync(vet);

            _examinationRepository
                .Setup(e => e.GetById(1))
                .ReturnsAsync(exam);

            var dto = new UpdateReportDto
            {
                PatientWeight = 10,
                Anamnesis = "test"
            };

            var result = await _reportService.UpdateReport(dto, 1, "test");
            Assert.NotNull(result);
            Assert.Equal(dto.PatientWeight, result.PatientWeight);
            Assert.Equal(dto.Anamnesis, result.Anamnesis);

            _reportRepository.Verify(r => r.UpdateReport(It.IsAny<ExamReport>()), Times.Once);
        }

        [Fact]
        public async Task UpdateReport_CantChange_ThrowBadRequestException()
        {
            var report = new ExamReport { Id = 1, ExaminationId = 1, CreatedAt = DateTime.UtcNow.AddDays(-30) };

            _reportRepository
                .Setup(r => r.GetReportById(1))
                .ReturnsAsync(report);
            
            var dto = new UpdateReportDto
            {
                PatientWeight = 10,
                Anamnesis = "test"
            };

            await Should.ThrowAsync<BadRequestException>(
                () => _reportService.UpdateReport(dto, 1, "test")
            );
        }
        [Fact]
        public async Task UpdateReport_Successfull()
        {
            var report = new ExamReport { Id = 1, ExaminationId = 1, CreatedAt = DateTime.UtcNow };
            var exam = new Examination { Id = 1 };
            var vet = new Vet
            {
                Id = 1,
                User = new ApplicationUser
                {
                    Name = "test",
                    Surname = "test",
                    UserName = "test"
                },
                Examinations = new List<Examination>()
            };
            vet.Examinations.Add(exam);
            _reportRepository
                .Setup(r => r.GetReportById(1))
                .ReturnsAsync(report);

            _vetRepository
                .Setup(v => v.FindByUsername("test"))
                .ReturnsAsync(vet);

            _examinationRepository
                .Setup(e => e.GetById(1))
                .ReturnsAsync(exam);

            var dto = new UpdateReportDto
            {
                PatientWeight = 10,
                Anamnesis = "test"
            };

            var result = await _reportService.UpdateReport(dto, 1, "test");
            Assert.NotNull(result);
            Assert.Equal(dto.PatientWeight, result.PatientWeight);
            Assert.Equal(dto.Anamnesis, result.Anamnesis);

            _reportRepository.Verify(r => r.UpdateReport(It.IsAny<ExamReport>()), Times.Once);
        }
    }
}
