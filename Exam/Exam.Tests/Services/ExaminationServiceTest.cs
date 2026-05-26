using AutoMapper;
using Exam.App.Domain;
using Exam.App.Domain.Repositories;
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
    public class ExaminationServiceTest
    {
        private readonly Mock<IExaminationRepository> _examRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IVetRepository> _vetRepository;
        private readonly IExaminationService _examService;
        public ExaminationServiceTest()
        {
            _examRepository = new Mock<IExaminationRepository>();
            _mapper = new Mock<IMapper>();
            _vetRepository = new Mock<IVetRepository>();

            _examService = new ExaminationService
           (
               _examRepository.Object,
            _mapper.Object,
            _vetRepository.Object
          );
        }

        [Fact]
        public async Task CreateExamination_Should_Throw_BadRequest_VetIsNotAvaliable()
        {
            var existingExam = new Examination
            {
                Id = 1,
                ExaminationDate = DateTime.UtcNow,
                VetId = 1,
                PetId = 1,
                Status = ExaminationStatus.Active
            };

            var vet = new Vet
            {
                Id = 1,
                Patients = new List<Patient> { new Patient { Id = 1 } },
                Examinations = new List<Examination> { existingExam }
            };

            _vetRepository
                .Setup(v => v.FindById(1))
                .ReturnsAsync(vet);

            var dto = new ExaminationDto
            {
                ExaminationDate = DateTime.UtcNow.AddMinutes(10),
                VetId = 1,
                PetId = 1
            };

            await Should.ThrowAsync<BadRequestException>(async () => await _examService.CreateExamination(dto));
        }
        [Fact]
        public async Task CreateExamination_VetIsAvaliable_Success()
        {
            var existingExam = new Examination
            {
                Id = 1,
                ExaminationDate = DateTime.UtcNow,
                VetId = 1,
                PetId = 1,
                Status = ExaminationStatus.Active
            };

            var vet = new Vet
            {
                Id = 1,
                Patients = new List<Patient> { new Patient { Id = 1 } },
                Examinations = new List<Examination> { existingExam }
            };

            _vetRepository
                .Setup(v => v.FindById(1))
                .ReturnsAsync(vet);

            var dto = new ExaminationDto
            {
                ExaminationDate = DateTime.UtcNow.AddDays(10),
                VetId = 1,
                PetId = 1
            };

            _mapper
               .Setup(m => m.Map<Examination>(It.IsAny<ExaminationDto>()))
               .Returns(new Examination
               {
                  ExaminationDate = dto.ExaminationDate
               });

            await _examService.CreateExamination(dto);
            _examRepository.Verify(e=> e.CreateExamination(It.IsAny<Examination>()), Times.Once);
        }
    }
}
