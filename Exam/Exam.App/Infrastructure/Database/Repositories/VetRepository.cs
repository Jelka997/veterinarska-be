using Exam.App.Domain;
using Exam.App.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Exam.App.Infrastructure.Database.Repositories
{
    public class VetRepository : IVetRepository
    {
        private readonly AppDbContext _context;

        public VetRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Vet?> FindByUsername(string username)
        {
            return await _context.Vets
                .Include(v => v.User)
                .Include(v => v.Patients)
                .ThenInclude(v => v.AnimalSpecie)
               .FirstOrDefaultAsync(v => v.User.UserName == username);
        }

        public async Task<Vet?> FindById(int id)
        {
            return await _context.Vets
                .Include(v => v.User)
                .Include(v => v.Patients)
                .ThenInclude(v => v.AnimalSpecie)
               .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<List<Vet>> GetAllVets()
        {
            return await _context.Vets
                .Include(v => v.User)
                .Include(v => v.Patients)
                .ToListAsync();
        }
    }
}
