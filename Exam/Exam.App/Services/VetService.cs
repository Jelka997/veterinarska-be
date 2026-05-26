using AutoMapper;
using Exam.App.Domain;
using Exam.App.Domain.Repositories;
using Exam.App.Services.Dtos;
using Exam.App.Services.Exceptions;
using Exam.App.Services.Interfaces;

namespace Exam.App.Services
{
    public class VetService : IVetService
    {
        private readonly IVetRepository _vetRepository;
        private readonly IMapper _mapper;

        public VetService(IVetRepository vetRepository, IMapper mapper)
        {
            _vetRepository = vetRepository;
            _mapper = mapper;
        }

        public async Task<List<VetPreviewDto>> GetAllVets()
        {
            var existingVets = await _vetRepository.GetAllVets();
            return existingVets.Select(_mapper.Map<VetPreviewDto>).ToList();
        }

        public async Task<VetByIdDto> GetVetById(int vetId)
        {
            var existingVets = await _vetRepository.FindById(vetId);
            if (existingVets == null) { throw new NotFoundException(vetId); }
            existingVets.Examinations.GroupBy(e => e.ExaminationDate);//mozda treba na klijentu ovo
            return _mapper.Map<VetByIdDto>(existingVets);
        }
    }
}
