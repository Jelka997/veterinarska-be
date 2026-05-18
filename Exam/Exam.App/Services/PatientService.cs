using AutoMapper;
using Exam.App.Domain;
using Exam.App.Domain.Repositories;
using Exam.App.Infrastructure.Database.Repositories;
using Exam.App.Services.Dtos;
using Exam.App.Services.Exceptions;
using Exam.App.Services.Interfaces;
using Exam.App.Utilis;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Exam.App.Services
{
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPatientRepository _patientRepo;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IVetRepository _vetRepository;
        private readonly IMapper _mapper;

        public PatientService(IUnitOfWork unitOfWork, IPatientRepository patientRepo, IMapper mapper, IOwnerRepository ownerRepository, IVetRepository vetRepository)
        {
            _unitOfWork = unitOfWork;
            _patientRepo = patientRepo;
            _mapper = mapper;
            _ownerRepository = ownerRepository;
            _vetRepository = vetRepository;
        }

        public async Task<CreatePatientDto> CreateNewPatient(CreatePatientDto dto)
        {
            var owner = await _ownerRepository.FindByUsername(dto.OwnerUsername);// ni ovo mozda ne treba jer repo sadrzi samo tu jednu metodu mogli smo preko user da nadjemo 
            if (owner == null)
            {
                throw new Exception("Owner not found.");
            }
            var newPatient = _mapper.Map<Patient>(dto);
            newPatient.OwnerId = owner.Id;

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _patientRepo.CreateNewPatient(newPatient);
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
            var result = _mapper.Map<CreatePatientDto>(newPatient);
            result.OwnerUsername = dto.OwnerUsername;
            return result;
        }

        public async Task<UpdatePatientDto> UpdatePatient(UpdatePatientDto dto, int id)
        {
            var existingPatient = await _patientRepo.GetPatientById(id);
            if (existingPatient == null)
            {
                throw new NotFoundException(id);
            }

            if (dto.VetId.HasValue)
            {
                var vet = await _vetRepository.FindById(dto.VetId.Value);

                if (vet == null)
                {
                    throw new NotFoundException(dto.VetId.Value);
                }

                existingPatient.VetId = dto.VetId.Value;
            }

            existingPatient.Name = dto.Name;
            existingPatient.DateOfBirth = dto.DateOfBirth;
            existingPatient.AnimalSpecieId = dto.AnimalSpecieId;
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _patientRepo.UpdatePatient(existingPatient);
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
            return _mapper.Map<UpdatePatientDto>(existingPatient);
        }

        public async Task<bool> DeletePatient(int id)
        {
            var existingPatient = await _patientRepo.GetPatientById(id);
            if (existingPatient == null)
            {
                throw new NotFoundException(id);
            }
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _patientRepo.DeletePatient(existingPatient);
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
            return true;
        }

        public async Task<PatientPreviewDto> GetOnePatient(int id)
        {
            var existingPatient = await _patientRepo.GetPatientById(id);
            if (existingPatient == null)
            {
                throw new NotFoundException(id);
            }
            return _mapper.Map<PatientPreviewDto>(existingPatient);
        }

        public async Task<List<PatientPreviewDto>> GetAllPatients()
        {
            var existingPatients = await _patientRepo.GetAllPatients();
            return existingPatients.Select(_mapper.Map<PatientPreviewDto>).ToList();
        }

        public async Task<PaginatedList<PatientPreviewDto>> GetAllFilterdPatients(int page, int pageSize, PatientSearchQuery patientSearchQuery)
        {
            var patients = await _patientRepo.GetAllFilterdPatients(page, pageSize, patientSearchQuery);
            var dtos = patients.Items
                .Select(_mapper.Map<PatientPreviewDto>).ToList();
            return new PaginatedList<PatientPreviewDto>(dtos, patients.Count, patients.PageIndex, pageSize);
        }
    }
}
