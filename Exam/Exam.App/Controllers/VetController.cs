using Exam.App.Services.Dtos;
using Exam.App.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Exam.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VetController : ControllerBase
    {
        private readonly IVetService _vetService;

        public VetController(IVetService vetService)
        {
            _vetService = vetService;
        }

        [Authorize(Roles = "Vet, Assistant")]
        [HttpGet]
        public async Task<List<VetPreviewDto>> GetAllVets()
        {
            return await _vetService.GetAllVets();
        }

        [Authorize(Roles = "Vet, Assistant")]
        [HttpGet("{vetId}")]
        public async Task<VetByIdDto> GetVetById(int vetId)
        {
            return await _vetService.GetVetById(vetId);
        }
    }
}
