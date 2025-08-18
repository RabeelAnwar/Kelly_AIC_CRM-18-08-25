using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Lead.DTOs;
using Service.Services.Lead;
using Service.Services.CallRecord.DTOs;
using Service.Services.CallRecord;

namespace AIC_CRM_API.Controllers
{
    //[AllowAnonymous]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LeadController : ControllerBase
    {
        private readonly ILeadService _leadService;


        public LeadController(ILeadService leadService)
        {
            _leadService = leadService;
        }

        [HttpPost("LeadAddUpdate")]
        public async Task<IActionResult> LeadAddUpdate([FromBody] LeadInput input)
        {
            var result = await _leadService.LeadAddUpdate(input);
            return Ok(result);
        }

        [HttpDelete("LeadDelete")]
        public async Task<IActionResult> LeadDelete(int id)
        {
            var result = await _leadService.LeadDelete(id);
            return Ok(result);
        }

        [HttpGet("LeadsListGet")]
        public async Task<IActionResult> LeadsListGet()
        {
            var result = await _leadService.LeadsListGet();
            return Ok(result);
        }


        [HttpGet("DetailedLeadsListGet")]
        public async Task<IActionResult> DetailedLeadsListGet()
        {
            var result = await _leadService.DetailedLeadsListGet();
            return Ok(result);
        }

        [HttpGet("SingleLeadGet")]
        public async Task<IActionResult> SingleLeadGet()
        {
            var result = await _leadService.SingleLeadGet();
            return Ok(result);
        }

    }
}
