using Exam.App.Domain;

namespace Exam.App.Domain.Repositories
{
    public interface IVetRepository
    {
        Task<Vet?> FindByUsername(string username);
        Task<Vet?> FindById(int id);
        Task<List<Vet>> GetAllVets();
    }
}