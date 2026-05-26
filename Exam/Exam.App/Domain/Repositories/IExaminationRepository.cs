using Exam.App.Domain;

namespace Exam.App.Domain.Repositories
{
    public interface IExaminationRepository
    {
        Task<Examination> CreateExamination(Examination examination);
        Task<Examination> UpdateExamination(Examination examination);
        Task<Examination?> GetById(int examinationId);
    }
}