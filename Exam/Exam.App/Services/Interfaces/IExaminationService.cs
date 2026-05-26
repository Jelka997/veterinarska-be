using Exam.App.Services.Dtos;

namespace Exam.App.Services.Interfaces
{
    public interface IExaminationService
    {
        Task<ExaminationDto> CreateExamination(ExaminationDto dto);
        Task<string> CancelExamination(string reason, int id);
    }
}