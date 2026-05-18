using Exam.App.Services.Dtos;
using Exam.App.Services.Interfaces;
using Exam.App.Utilis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;

namespace Exam.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [Authorize(Roles = "Vet, Assistant")]
        [HttpPost]
        public async Task<ActionResult<CreatePatientDto>> CreateNewPatient(CreatePatientDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdPatient = await _patientService.CreateNewPatient(dto);
            return Created(string.Empty, createdPatient);
        }

        [Authorize(Roles = "Vet, Assistant")]
        [HttpPut("{id}")]
        public async Task<ActionResult<UpdatePatientDto>> UpdatePatient(UpdatePatientDto dto, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _patientService.UpdatePatient(dto, id);
            return Ok(result);
        }

        [Authorize(Roles = "Vet, Assistant")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeletePatient(int id)
        {
            var result = await _patientService.DeletePatient(id);
            return Ok(result);
        }

        [Authorize(Roles = "Vet, Assistant")]
        [HttpGet("{id}")]
        public async Task<ActionResult<PatientPreviewDto>> GetOnePatient(int id)
        {
            var result = await _patientService.GetOnePatient(id);
            return Ok(result);
        }

        [Authorize(Roles = "Vet, Assistant")]
        [HttpGet]
        public async Task<ActionResult<List<PatientPreviewDto>>> GetAllPatients()
        {
            var result = await _patientService.GetAllPatients();
            return Ok(result);
        }

        [Authorize(Roles = "Vet, Assistant")]
        [HttpGet("filter")]
        public async Task<ActionResult<PaginatedList<PatientPreviewDto>>> GetAllFilterdPatients([FromQuery] PatientSearchQuery query,[FromQuery] int page = 1,[FromQuery] int pageSize = 5)
        {
            var result = await _patientService.GetAllFilterdPatients(page, pageSize, query);
            return Ok(result);
        }

    }
}
