using Exam.App.Domain;

namespace Exam.App.Domain.Repositories
{
    public interface IOwnerRepository
    {
        Task<Owner?> FindByUsername(string username);
    }
}