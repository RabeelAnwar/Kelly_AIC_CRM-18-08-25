using Microsoft.AspNetCore.Mvc;
using Service.Converter;
using Service.Services.Consultant;
using Service.Services.Consultant.DTOs;

namespace AIC_CRM_API.Controllers
{
    //[AllowAnonymous]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultantController : ControllerBase
    {
        private readonly IConsultantService _consultantService;

        public ConsultantController(IConsultantService consultantService)
        {
            _consultantService = consultantService;
        }

        [Consumes("multipart/form-data")]
        [HttpPost("ConsultantAddUpdate")]
        public async Task<IActionResult> ConsultantAddUpdate([FromForm] ConsultantDto input)
        {
            var result = await _consultantService.ConsultantAddUpdate(input);
            return Ok(result);
        }

        [HttpDelete("ConsultantDelete")]
        public async Task<IActionResult> ConsultantDelete(int id)
        {
            var result = await _consultantService.ConsultantDelete(id);
            return Ok(result);
        }

        [HttpGet("ConsultantsListGet")]
        public async Task<IActionResult> ConsultantsListGet()
        {
            var result = await _consultantService.ConsultantsListGet();
            return Ok(result);
        }

        [HttpGet("SingleConsultantGet")]
        public async Task<IActionResult> SingleConsultantGet(int id)
        {
            var result = await _consultantService.SingleConsultantGet(id);
            return Ok(result);
        }


        [HttpDelete("DeleteConsultantFileAsync")]
        public async Task<IActionResult> DeleteConsultantFileAsync(int id)
        {
            var result = await _consultantService.DeleteConsultantFileAsync(id);
            return Ok(result);
        }

        [HttpGet("ExtractText")]
        public async Task<IActionResult> ExtractText(string url)
        {
            var result = await _consultantService.ExtractTextAsync(url);
            return Ok(result);
        }


        #region Consultant Interview Process

        [HttpPost("InterviewProcessAddUpdate")]
        public async Task<IActionResult> InterviewProcessAddUpdate([FromBody] ConsultantInterviewProcessDto input)
        {
            var result = await _consultantService.InterviewProcessAddUpdate(input);
            return Ok(result);
        }


        [HttpDelete("InterviewProcessDelete")]
        public async Task<IActionResult> InterviewProcessDelete(int id)
        {
            var result = await _consultantService.InterviewProcessDelete(id);
            return Ok(result);
        }



        #endregion

        #region Consultant Activity


        [HttpPost("ConsultantActivityAddUpdate")]
        public async Task<IActionResult> ConsultantActivityAddUpdate([FromBody] ConsultantActivityDto input)
        {
            var result = await _consultantService.ConsultantActivityAddUpdate(input);
            return Ok(result);
        }


        [HttpDelete("ConsultantActivityDelete")]
        public async Task<IActionResult> DeleteConsultantConsultantActivityDeleteFileAsync(int id)
        {
            var result = await _consultantService.ConsultantActivityDelete(id);
            return Ok(result);
        }

        [HttpGet("SearchConsultantsForActivity")]
        public async Task<IActionResult> SearchConsultantsForActivity(string name)
        {
            var result = await _consultantService.SearchConsultantsForActivity(name);
            return Ok(result);
        }


        [HttpGet("InterviewProcessConsultantsList")]
        public async Task<IActionResult> InterviewProcessConsultantsList(int? requisitionId)
        {
            var result = await _consultantService.InterviewProcessConsultantsList(requisitionId);
            return Ok(result);
        }


        [HttpGet("InterviewProcessConsultantSingle")]
        public async Task<IActionResult> InterviewProcessConsultantSingle(int id)
        {
            var result = await _consultantService.InterviewProcessConsultantSingle(id);
            return Ok(result);
        }



        #endregion


        #region Interview

        [HttpPost("InterviewAddUpdate")]
        public async Task<IActionResult> InterviewAddUpdate([FromBody] ConsultantInterviewDto input)
        {
            var result = await _consultantService.InterviewAddUpdate(input);
            return Ok(result);
        }

        [HttpDelete("InterviewDelete")]
        public async Task<IActionResult> InterviewDelete(int id)
        {
            var result = await _consultantService.InterviewDelete(id);
            return Ok(result);
        }

        [HttpGet("InterviewListGet")]
        public async Task<IActionResult> InterviewListGet()
        {
            var result = await _consultantService.InterviewListGet();
            return Ok(result);
        }

        #endregion

        #region Reference

        [HttpPost("ReferenceAddUpdate")]
        public async Task<IActionResult> ReferenceAddUpdate([FromBody] ConsultantReferenceDto input)
        {
            var result = await _consultantService.ReferenceAddUpdate(input);
            return Ok(result);
        }

        [HttpDelete("ReferenceDelete")]
        public async Task<IActionResult> ReferenceDelete(int id)
        {
            var result = await _consultantService.ReferenceDelete(id);
            return Ok(result);
        }

        [HttpGet("ReferenceListGet")]
        public async Task<IActionResult> ReferenceListGet()
        {
            var result = await _consultantService.ReferenceListGet();
            return Ok(result);
        }

        #endregion


    }
}
