using Exam.App.Domain;
using Exam.App.Domain.Repositories;
using Exam.App.Services.Dtos;
using Exam.App.Utilis;
using Microsoft.EntityFrameworkCore;

namespace Exam.App.Infrastructure.Database.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly AppDbContext _context;

        public PatientRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Patient> CreateNewPatient(Patient patient)
        {
            _context.Patients.Add(patient);
            //await _context.SaveChangesAsync();
            return patient;
        }
        public async Task<Patient?> GetPatientById(int id)
        {
            return await _context.Patients
                .Include(p => p.AnimalSpecie)
                .Include(p => p.Owner)
                 .ThenInclude(o => o.User)
                .Include(p => p.Vet)
                  .ThenInclude(v => v.User)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<Patient> UpdatePatient(Patient patient)
        {
            _context.Patients.Update(patient);
            //await _context.SaveChangesAsync();
            return patient;
        }
        public async Task<bool> DeletePatient(Patient patient)
        {
            _context.Patients.Remove(patient);
            //await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Patient>> GetAllPatients()
        {
            return await _context.Patients
                .Include(p => p.Owner)
                 .ThenInclude(o => o.User)
                .Include(p => p.Vet)
                 .ThenInclude(v => v.User)
                .Include(p => p.AnimalSpecie)
                .ToListAsync();
        }

        public async Task<PaginatedList<Patient>> GetAllFilterdPatients(int page, int pageSize,PatientSearchQuery patientSearchQuery)
        {
            IQueryable<Patient> patients = _context.Patients
           .Include(p => p.Owner)
           .ThenInclude(o => o.User)
           .Include(p => p.Vet)
           .ThenInclude(v => v.User)
           .Include(p => p.AnimalSpecie);

            patients = FilterPatients(patients, patientSearchQuery);
            int pageIndex = page - 1;
            var count = await patients.CountAsync();
            var items = await patients.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<Patient>(items, count, pageIndex, pageSize);
        }

        private IQueryable<Patient> FilterPatients(IQueryable<Patient> patients, PatientSearchQuery patientSearchQuery)
        {
            if (!string.IsNullOrEmpty(patientSearchQuery.VetName))
            {
                patients = patients.Where(p => p.Vet.User.Name.ToLower().Contains(patientSearchQuery.VetName.ToLower()));
            }

            if (!string.IsNullOrEmpty(patientSearchQuery.PatName))
            {
                patients = patients.Where(p => p.Name.ToLower().Contains(patientSearchQuery.PatName.ToLower()));
            }

            if (!string.IsNullOrEmpty(patientSearchQuery.AnimalSpecie))
            {
                patients = patients.Where(p => p.AnimalSpecie.Name.ToLower().Contains(patientSearchQuery.AnimalSpecie.ToLower()));
            }

            if (patientSearchQuery.AgeFrom != null)
            {
                patients = patients.Where(p => (DateTime.Now.Year - p.DateOfBirth.Year) >= patientSearchQuery.AgeFrom);
            }

            if (patientSearchQuery.AgeTo != null)
            {
                patients = patients.Where(p => (DateTime.Now.Year - p.DateOfBirth.Year) <= patientSearchQuery.AgeTo);
            }

            return patients;
        }
    }
}
