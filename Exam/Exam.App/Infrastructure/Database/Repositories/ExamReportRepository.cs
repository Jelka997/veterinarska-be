using Exam.App.Domain;
using Exam.App.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Exam.App.Infrastructure.Database.Repositories
{
    public class ExamReportRepository : IExamReportRepository
    {
        private readonly AppDbContext _context;

        public ExamReportRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ExamReport> CreateReport(ExamReport report)
        {
            _context.ExamReports.Add(report);
            await _context.SaveChangesAsync();
            return report;
        }

        public async Task<ExamReport> UpdateReport(ExamReport report)
        {
            _context.ExamReports.Update(report);
            await _context.SaveChangesAsync();
            return report;
        }

        public async Task<ExamReport?> GetReportById(int id)
        {
            return await _context.ExamReports
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
