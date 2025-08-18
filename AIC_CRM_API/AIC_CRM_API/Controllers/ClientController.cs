using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Client;
using Service.Services.Client.DTOs;
using Service.Services.Requisition;
using Service.Services.Requisition.DTOs;

namespace AIC_CRM_API.Controllers
{
    //[AllowAnonymous]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly IRequisitionService _requisitionService;

        public ClientController(IClientService clientService, IRequisitionService requisitionService)
        {
            _clientService = clientService;
            _requisitionService = requisitionService;
        }

        [HttpPost("ClientAddUpdate")]
        public async Task<IActionResult> ClientAddUpdate([FromBody] ClientInput input)
        {
            var result = await _clientService.ClientAddUpdate(input);
            return Ok(result);
        }

        [HttpDelete("ClientDelete")]
        public async Task<IActionResult> ClientDelete(int id)
        {
            var result = await _clientService.ClientDelete(id);
            return Ok(result);
        }

        [HttpGet("ClientsListGet")]
        public async Task<IActionResult> ClientsListGet()
        {
            var result = await _clientService.ClientsListGet();
            return Ok(result);
        }

        [HttpGet("SingleClientGet")]
        public async Task<IActionResult> SingleClientGet(int id)
        {
            var result = await _clientService.SingleClientGet(id);
            return Ok(result);
        }

        #region Client Manager

        [HttpPost("ClientManagerAddUpdate")]
        public async Task<IActionResult> ClientManagerAddUpdate([FromBody] ClientManagerDto input)
        {
            var result = await _clientService.ClientManagerAddUpdate(input);
            return Ok(result);
        }

        [HttpDelete("ClientManagerDelete")]
        public async Task<IActionResult> ClientManagerDelete(int id)
        {
            var result = await _clientService.ClientManagerDelete(id);
            return Ok(result);
        }

        [HttpGet("ClientManagersListGet")]
        public async Task<IActionResult> ClientManagersListGet()
        {
            var result = await _clientService.ClientManagersListGet();
            return Ok(result);
        }

        [HttpGet("ClientManagerGet")]
        public async Task<IActionResult> ClientManagerGet(int id)
        {
            var result = await _clientService.ClientManagerGet(id);
            return Ok(result);
        }


        [HttpGet("ClientManagerGetByClientId")]
        public async Task<IActionResult> ClientManagerGetByClientId(int id)
        {
            var result = await _clientService.ClientManagerGetByClientId(id);
            return Ok(result);
        }

        [HttpGet("GetWorkUnderManagers")]
        public async Task<IActionResult> GetWorkUnderManagers(int id)
        {
            var result = await _clientService.GetWorkUnderManagers(id);
            return Ok(result);
        }

        #endregion

        #region Client Pipeline

        [HttpPost("ClientPipelineAddUpdate")]
        public async Task<IActionResult> ClientPipelineAddUpdate([FromBody] ClientPipelineDto input)
        {
            var result = await _clientService.ClientPipelineAddUpdate(input);
            return Ok(result);
        }

        [HttpDelete("ClientPipelineDelete")]
        public async Task<IActionResult> ClientPipelineDelete(int id)
        {
            var result = await _clientService.ClientPipelineDelete(id);
            return Ok(result);
        }

        [HttpGet("ClientPipelinesListGet")]
        public async Task<IActionResult> ClientPipelinesListGet()
        {
            var result = await _clientService.ClientPipelinesListGet();
            return Ok(result);
        }

        [HttpGet("ClientPipelineGet")]
        public async Task<IActionResult> ClientPipelineGet(int id)
        {
            var result = await _clientService.ClientPipelineGet(id);
            return Ok(result);
        }

        #endregion

        #region Client Requisition

        //[HttpPost("ClientRequisitionAddUpdate")]
        //public async Task<IActionResult> ClientRequisitionAddUpdate([FromBody] RequisitionDto input)
        //{
        //    var result = await _requisitionService.ClientRequisitionAddUpdate(input);
        //    if (!result.Succeeded)
        //    {
        //        // Log what failed and why
        //        Console.WriteLine("Save failed: " + result.Message);
        //    }
        //    return Ok(result);
        //}
        [HttpPost("ClientRequisitionAddUpdate")]
        public async Task<IActionResult> ClientRequisitionAddUpdate([FromBody] RequisitionDto input)
        {
            try
            {
                var result = await _requisitionService.ClientRequisitionAddUpdate(input);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error in API: " + ex);
                return StatusCode(500, new { message = ex.Message, detail = ex.StackTrace });
            }
        }

        [HttpDelete("ClientRequisitionDelete")]
        public async Task<IActionResult> ClientRequisitionDelete(int id)
        {
            var result = await _requisitionService.ClientRequisitionDelete(id);
            return Ok(result);
        }


        [HttpPost("ClientRequisitionStatusUpdate")]
        public async Task<IActionResult> ClientRequisitionStatusUpdate([FromBody] RequisitionDto input)
        {
            var result = await _requisitionService.ClientRequisitionStatusUpdate(input);
            return Ok(result);
        }


        [HttpGet("ClientRequisitionsListGet")]
        public async Task<IActionResult> ClientRequisitionsListGet(int? clientId)
        {
            if (clientId.HasValue && clientId.Value > 0)
            {
                var resultC = await _requisitionService.ClientRequisitionsListGetByClientId(clientId.Value);
                return Ok(resultC);
            }
            var result = await _requisitionService.ClientRequisitionsListGet();
            return Ok(result);
        }

        [HttpGet("ClientRequisitionGet")]
        public async Task<IActionResult> ClientRequisitionGet(int id)
        {
            var result = await _requisitionService.ClientRequisitionGet(id);
            return Ok(result);
        }
        
        
        [HttpGet("ClientOpenRequisitionsListGet")]
        public async Task<IActionResult> ClientOpenRequisitionsListGet()
        {
            var result = await _requisitionService.ClientOpenRequisitionsListGet();
            return Ok(result);
        }

        [HttpGet("ClientRequisitionGetByClientId")]
        public async Task<IActionResult> ClientRequisitionGetByClientId(int id)
        {
            var result = await _requisitionService.ClientRequisitionGetByClientId(id);
            return Ok(result);
        }

        #endregion

    }

}
