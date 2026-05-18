using Exam.App.Domain;
using Exam.App.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Exam.App.Infrastructure.Database.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly AppDbContext _context;

        public OwnerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Owner?> FindByUsername(string username)
        {
            return await _context.Owners
                .Include(o => o.User)
                .Include(o => o.Pats)
                .ThenInclude(p => p.AnimalSpecie)
               .FirstOrDefaultAsync(o => o.User.UserName == username);
        }
    }
}
