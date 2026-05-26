using Exam.App.Domain;

namespace Exam.App.Domain.Repositories
{
    public interface IExamReportRepository
    {
        Task<ExamReport> CreateReport(ExamReport report);
        Task<ExamReport> UpdateReport(ExamReport report);
        Task<ExamReport?> GetReportById(int id);
    }
}