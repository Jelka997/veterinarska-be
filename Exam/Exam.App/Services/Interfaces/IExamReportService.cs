using Exam.App.Services.Dtos;

namespace Exam.App.Services.Interfaces
{
    public interface IExamReportService
    {
        Task<ExamReportDto> CreateReport(ExamReportDto dto);
        Task<UpdateReportDto> UpdateReport(UpdateReportDto dto, int reportId, string vetId);
    }
}