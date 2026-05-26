using AutoMapper;
using Exam.App.Domain;
using Exam.App.Domain.Repositories;
using Exam.App.Services.Dtos;
using Exam.App.Services.Exceptions;
using Exam.App.Services.Interfaces;
using System.Runtime.CompilerServices;

namespace Exam.App.Services
{
    public class ExaminationService : IExaminationService
    {
        private readonly IExaminationRepository _examRepository;
        private readonly IMapper _mapper;
        private readonly IVetRepository _vetRepository;

        public ExaminationService(IExaminationRepository examRepository, IMapper mapper, IVetRepository vetRepository)
        {
            _examRepository = examRepository;
            _mapper = mapper;
            _vetRepository = vetRepository;
        }

        public async Task<ExaminationDto> CreateExamination(ExaminationDto dto)
        {
            var existingVet = await _vetRepository.FindById(dto.VetId);
            if (existingVet == null)
            {
                throw new NotFoundException(dto.VetId);
            }
            var vetPatients = existingVet.Patients;
            if (!vetPatients.Any(p => p.Id == dto.PetId))
            {
                throw new BadRequestException("This patient doesn`t belong to this vet.");
            }
            var examinations = existingVet.Examinations
                .Where(e => e.Status == ExaminationStatus.Active);
            foreach (var examination in examinations)
            {
                if (dto.ExaminationDate < examination.ExaminationDate.AddMinutes(20) && examination.ExaminationDate < dto.ExaminationDate.AddMinutes(20))
                {
                    throw new BadRequestException("Vet is not avaliable for chosen date.");
                }
            }
            var response = _mapper.Map<Examination>(dto);
            response.Status = ExaminationStatus.Active;
            await _examRepository.CreateExamination(response);
            return dto;
        }
        public async Task<string> CancelExamination(string reason, int id)
        {
            var existingExamination = await _examRepository.GetById(id);
            if (existingExamination == null) { throw new NotFoundException(id); }
            existingExamination.CancellationReason = reason;
            existingExamination.Status = ExaminationStatus.Cancelled;
            await _examRepository.UpdateExamination(existingExamination);
            return reason;
        }
    }

}