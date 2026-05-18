using Exam.App.Domain;
using Exam.App.Services.Dtos;
using Exam.App.Utilis;

namespace Exam.App.Services.Interfaces
{
    public interface IPatientService
    {
        Task<CreatePatientDto> CreateNewPatient(CreatePatientDto createPatientDto);
        Task<UpdatePatientDto> UpdatePatient(UpdatePatientDto dto, int id);
        Task<bool> DeletePatient(int id);
        Task<PatientPreviewDto> GetOnePatient(int id);
        Task<List<PatientPreviewDto>> GetAllPatients();
        Task<PaginatedList<PatientPreviewDto>> GetAllFilterdPatients(int page, int pageSize ,PatientSearchQuery patientSearchQuery);
    }
}
