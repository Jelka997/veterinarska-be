using Exam.App.Domain;
using Exam.App.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Exam.App.Infrastructure.Database.Repositories
{
    public class ExaminationRepository : IExaminationRepository
    {
        private readonly AppDbContext _context;

        public ExaminationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Examination> CreateExamination(Examination examination)
        {
            _context.Examinations.Add(examination);
            await _context.SaveChangesAsync();
            return examination;
        }
        public async Task<Examination> UpdateExamination(Examination examination)
        {
            _context.Examinations.Update(examination);
            await _context.SaveChangesAsync();
            return examination;
        }
        public async Task<Examination?> GetById(int examinationId)
        {
            return await _context.Examinations
                 .FirstOrDefaultAsync(e => e.Id == examinationId);
        }
    }
}
