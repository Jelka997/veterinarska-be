using Exam.App.Services.Dtos;

namespace Exam.App.Services.Interfaces
{
    public interface IVetService
    {
        Task<List<VetPreviewDto>> GetAllVets();
    }
}