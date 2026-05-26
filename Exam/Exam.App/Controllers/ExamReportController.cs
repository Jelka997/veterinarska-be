using Exam.App.Services.Dtos;
using Exam.App.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Exam.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamReportController : ControllerBase
    {
        private readonly IExamReportService _examReportService;

        public ExamReportController(IExamReportService examReportService)
        {
            _examReportService = examReportService;
        }

        [Authorize(Roles = "Vet")]
        [HttpPost]
        public async Task<ActionResult<ExamReportDto>> CreateReport(ExamReportDto reportDto)
        {
            return await _examReportService.CreateReport(reportDto);
        }

        [Authorize(Roles = "Vet")]
        [HttpPut("{reportId}")]
        public async Task<ActionResult<UpdateReportDto>> UpdateReport(UpdateReportDto reportDto, int reportId)
        {
            var vetId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return await _examReportService.UpdateReport(reportDto, reportId, vetId);
        }
    }
}
