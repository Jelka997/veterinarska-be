using Exam.App.Domain;
using Exam.App.Services.Dtos;
using Exam.App.Utilis;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exam.App.Domain.Repositories
{
    public interface IPatientRepository
    {
        Task<List<Patient>> GetAllPatients();
        Task<Patient?> GetPatientById(int id);
        Task<Patient> CreateNewPatient(Patient patient);
        Task<Patient> UpdatePatient(Patient patient);
        Task<bool> DeletePatient(Patient patient);
        Task<PaginatedList<Patient>> GetAllFilterdPatients(int page, int pageSize, PatientSearchQuery patientSearchQuery);
    }
}
