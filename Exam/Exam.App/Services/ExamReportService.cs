using AutoMapper;
using Exam.App.Domain;
using Exam.App.Domain.Repositories;
using Exam.App.Services.Dtos;
using Exam.App.Services.Exceptions;
using Exam.App.Services.Interfaces;

namespace Exam.App.Services
{
    public class ExamReportService : IExamReportService
    {
        private readonly IExamReportRepository _reportRepository;
        private readonly IExaminationRepository _examinationRepository;
        private readonly IVetRepository _vetRepository;
        private readonly IMapper _mapper;

        public ExamReportService(IExamReportRepository reportRepository, IExaminationRepository examinationRepository, IMapper mapper, IVetRepository vetRepository)
        {
            _reportRepository = reportRepository;
            _examinationRepository = examinationRepository;
            _mapper = mapper;
            _vetRepository = vetRepository;
        }

        public async Task<ExamReportDto> CreateReport(ExamReportDto dto)
        {
            var existingExam = await _examinationRepository.GetById(dto.ExaminationId);
            if (existingExam == null) { throw new NotFoundException(dto.ExaminationId); }
            
            if(existingExam.Status != ExaminationStatus.Active) { throw new BadRequestException("This examination is not active."); }
           
            var report = _mapper.Map<ExamReport>(dto);
            await _reportRepository.CreateReport(report);
            existingExam.Status = ExaminationStatus.Finished;
            await _examinationRepository.UpdateExamination(existingExam);
            return dto;
        }

        public async Task<UpdateReportDto> UpdateReport(UpdateReportDto dto, int reportId, string vetId)
        {
            var existingReport = await _reportRepository.GetReportById(reportId);
            var existingVet = await _vetRepository.FindByUsername(vetId); 
            if (existingReport == null) { throw new NotFoundException(reportId); }
            var existingExam = await _examinationRepository.GetById(existingReport.ExaminationId);
            if (existingVet == null) { throw new BadRequestException("Vet not found"); }
            if (!existingVet.Examinations.Any(e => e.Id == existingExam.Id))
            {
                throw new BadRequestException("This exam does not belong to chosen vet");
            }

            if (existingExam.Status == ExaminationStatus.Cancelled) { throw new BadRequestException("This examination is not active."); }

            existingReport.PatientWeight = dto.PatientWeight;
            existingReport.Anamnesis = dto.Anamnesis;
            if (!existingReport.CanUpdate()) { throw new BadRequestException("You can not update this report."); }
            await _reportRepository.UpdateReport(existingReport);
            return dto;
        }
    }
}
