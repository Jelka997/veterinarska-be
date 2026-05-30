using Exam.App.Services.Dtos;
using Exam.App.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Exam.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExaminationController : ControllerBase
    {
        private readonly IExaminationService _examinationService;

        public ExaminationController(IExaminationService examinationService)
        {
            _examinationService = examinationService;
        }

        [Authorize(Roles = "Assistant")]
        [HttpPost]
        public async Task<ExaminationDto> CreateExamination(ExaminationDto dto)
        {
            return await _examinationService.CreateExamination(dto);
        }

        [Authorize(Roles = "Vet")]
        [HttpPut("{id}")]
        public async Task<string> CancelExamination(int id,[FromBody] string reason)
        {
            return await _examinationService.CancelExamination(reason, id);
        }
    }
}
