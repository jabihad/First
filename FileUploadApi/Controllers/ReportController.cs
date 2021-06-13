using FileUploadApi.Model;
using FileUploadApi.Services.Report.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUploadApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        public ReportController(
            IReportService reportService
            )
        {
            _reportService = reportService;
        }

        [HttpPost("Generate")]
        public async Task<IEnumerable<ReportModel>> Generate(string email)
        {
            var result = await _reportService.Generate(email);
            return result;
        }
    }
}
