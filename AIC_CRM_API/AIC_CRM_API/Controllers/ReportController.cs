using Microsoft.AspNetCore.Mvc;
using Service.Services.Reports;
using Service.Services.Reports.DTOs;

namespace AIC_CRM_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ICrmReports _rpt;

        public ReportController(ICrmReports rpt)
        {
            _rpt = rpt;
        }


        [HttpPost("AllRecruitersRptGet")]
        public async Task<IActionResult> AllRecruitersRptGet(RecruitersRptDto input)
        {
            var result = await _rpt.AllRecruitersRptGet(input);
            return Ok(result);
        }


        [HttpPost("AllRecruitersRequisitionRptGet")]
        public async Task<IActionResult> AllRecruitersRequisitionRptGet(RecruitersRptDto input)
        {
            var result = await _rpt.AllRecruitersRequisitionRptGet(input);
            return Ok(result);
        }


        [HttpPost("IndividualReportRptGet")]
        public async Task<IActionResult> IndividualReportRptGet(RecruitersRptDto input)
        {
            var result = await _rpt.IndividualReportRptGet(input);
            return Ok(result);
        }  
        
        
        [HttpPost("IndividualRequisitionRptGet")]
        public async Task<IActionResult> IndividualRequisitionRptGet(RecruitersRptDto input)
        {
            var result = await _rpt.IndividualRequisitionRptGet(input);
            return Ok(result);
        }


        [HttpPost("OfficeMasterRptGet")]
        public async Task<IActionResult> OfficeMasterRptGet(MasterOfficeRptDto input)
        {
            var result = await _rpt.OfficeMasterRptGet(input);
            return Ok(result);
        }


        [HttpPost("AuditRptGet")]
        public async Task<IActionResult> AuditRptGet(AuditRptDto input)
        {
            var result = await _rpt.AuditRptGet(input);
            return Ok(result);
        }

    }
}
