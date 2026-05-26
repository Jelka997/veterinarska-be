using AutoMapper;
using Exam.App.Domain;
using Exam.App.Domain.Repositories;
using Exam.App.Services;
using Exam.App.Services.Dtos;
using Exam.App.Services.Exceptions;
using Exam.App.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Moq;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Tests.Services
{
    public class PatientServiceTets
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IPatientRepository> _patientRepository;
        private readonly Mock<IOwnerRepository> _ownerRepository;
        private readonly Mock<IVetRepository> _vetRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly IPatientService _patientService;

        public PatientServiceTets()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _patientRepository = new Mock<IPatientRepository>();
            _ownerRepository = new Mock<IOwnerRepository>();
            _vetRepository = new Mock<IVetRepository>();
            _mapper = new Mock<IMapper>();

            _patientService = new PatientService(
                _unitOfWork.Object,
                _patientRepository.Object,
                _mapper.Object,
                _ownerRepository.Object,
                _vetRepository.Object
            );
        }

        [Fact]
        public async Task UpdatePatient_When_PatientNotFound_ThrowsNotFoundException()
        {
            _patientRepository
                .Setup(p => p.GetPatientById(1))
                .ReturnsAsync((Patient)null);

            var dto = new UpdatePatientDto
            {
                Name = "Test",
                DateOfBirth = DateTime.UtcNow,
                AnimalSpecieId = 1,
            };
            await Should.ThrowAsync<NotFoundException>(async () => await _patientService.UpdatePatient(dto, 1));
        }

        [Fact]
        public async Task UpdatePatient_WhenValid_UpdateSuccessfully()
        {
            var patient = new Patient { Id = 1, Name = "Rex", AnimalSpecieId = 1, DateOfBirth = DateTime.UtcNow, OwnerId = 1, VetId = null };
            var dto = new UpdatePatientDto
            {
                Name = "Test",
                DateOfBirth = DateTime.UtcNow,
                AnimalSpecieId = 1,
            };
            _patientRepository
                .Setup(p => p.GetPatientById(1))
                .ReturnsAsync(patient);

               _patientRepository
                .Setup(p => p.UpdatePatient(It.IsAny<Patient>()))
                .ReturnsAsync(patient);

            _mapper
                .Setup(m => m.Map<UpdatePatientDto>(It.IsAny<Patient>()))
                .Returns(new UpdatePatientDto
                {
                    Name = dto.Name,
                    DateOfBirth = dto.DateOfBirth,
                    AnimalSpecieId = dto.AnimalSpecieId
                });
            var result = await _patientService.UpdatePatient(dto, 1);
            Assert.NotNull(result);//proverava da li je vrednost null
            Assert.Equal(dto.Name, result.Name);
            Assert.Equal(dto.AnimalSpecieId, result.AnimalSpecieId);
            Assert.Equal(dto.DateOfBirth, result.DateOfBirth);

            _patientRepository.Verify(r => r.UpdatePatient(It.IsAny<Patient>()), Times.Once);
        }
    }
}
