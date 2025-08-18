using Microsoft.AspNetCore.Mvc;
using Service.Services.Admin;
using Service.Services.Admin.CallType;
using Service.Services.Admin.ContactType;
using Service.Services.Admin.Department;
using Service.Services.Admin.DocumentType;
using Service.Services.Admin.SkillMaster;
using Service.Services.User.UserMaster;
using Service.Services.Company;
using Service.Services.Company.DTOs;
using Service.Services.User;
using Service.Services.DocumentUpload;
using Service.Services.Consultant.DTOs;
using Service.Services.DocumentUpload.DTOs;
using DataAccess.Entities;
using Service.Services.CallRecord.DTOs;
using Service.Services.CallRecord;
using Service.Services.Dashboard;
using Service.Services.Admin.SupportTicket;

namespace AIC_CRM_API.Controllers
{
    //[AllowAnonymous]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ICompanyService _company;
        private readonly IAdminServices _adminServices;
        private readonly IUserMasterService _userMaster;
        private readonly IDocumentUploadService _documentUpload;
        private readonly ICallRecordService _callRecord;
        private readonly IDashboardService _dashboard;


        public AdminController(ICompanyService company,
            IAdminServices adminServices,
            IUserMasterService userMaster,
            IDocumentUploadService documentUpload,
            ICallRecordService callRecord,
            IDashboardService dashboardService)
        {
            _company = company;
            _adminServices = adminServices;
            _userMaster = userMaster;
            _documentUpload = documentUpload;
            _callRecord = callRecord;
            _dashboard = dashboardService;

        }


        #region Company

        [HttpPost("CompanyAddUpdate")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OutputDTO<bool>))]
        public async Task<IActionResult> CompanyAddUpdate(CompanyInput company)
        {
            var result = await _company.CompanyAddUpdate(company);
            return Ok(result);
        }

        [HttpDelete("CompanyDelete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OutputDTO<bool>))]
        public async Task<IActionResult> CompanyDelete(int companyId)
        {
            var result = await _company.CompanyDelete(companyId);
            return Ok(result);
        }


        [HttpGet("CompaniesListGet")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OutputDTO<bool>))]
        public async Task<IActionResult> CompaniesListGet()
        {
            var result = await _company.CompaniesListGet();
            return Ok(result);
        }

        [HttpGet("CompanyProfileGet")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OutputDTO<bool>))]
        public async Task<IActionResult> CompanyProfileGet()
        {
            var result = await _company.CompanyProfileGet();
            return Ok(result);
        }


        //[HttpPost("RoleBasePermission")]
        //[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OutputDTO<bool>))]
        //public async Task<IActionResult> RoleBasePermission(UserRegisterDto user)
        //{
        //    var result = await _auth.UserRegister(user);
        //    return Ok(result);
        //}

        #endregion


        #region Department

        [HttpPost("DepartmentAddUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OutputDTO<bool>))]
        public async Task<IActionResult> DepartmentAddUpdate([FromBody] DepartmentDto input)
        {
            var result = await _adminServices.DepartmentAddUpdate(input);
            return Ok(result);
        }

        [HttpDelete("DepartmentDelete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OutputDTO<bool>))]
        public async Task<IActionResult> DepartmentDelete(int id)
        {
            var result = await _adminServices.DepartmentDelete(id);
            return Ok(result);
        }

        [HttpGet("DepartmentGet")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OutputDTO<List<DepartmentDto>>))]
        public async Task<IActionResult> DepartmentGet()
        {
            var result = await _adminServices.DepartmentGet();
            return Ok(result);
        }


        #endregion


        #region Document Type

        [HttpPost("DocumentTypeAddUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OutputDTO<bool>))]
        public async Task<IActionResult> DocumentTypeAddUpdate([FromBody] DocumentTypeDto input)
        {
            var result = await _adminServices.DocumentTypeAddUpdate(input);
            return Ok(result);
        }

        [HttpDelete("DocumentTypeDelete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OutputDTO<bool>))]
        public async Task<IActionResult> DocumentTypeDelete(int id)
        {
            var result = await _adminServices.DocumentTypeDelete(id);
            return Ok(result);
        }

        [HttpGet("DocumentTypeGet")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OutputDTO<List<DocumentTypeDto>>))]
        public async Task<IActionResult> DocumentTypeGet()
        {
            var result = await _adminServices.DocumentTypeGet();
            return Ok(result);
        }

        #endregion


        #region Call Type

        [HttpPost("CallTypeAddUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OutputDTO<bool>))]
        public async Task<IActionResult> CallTypeAddUpdate([FromBody] CallTypeDto input)
        {
            var result = await _adminServices.CallTypeAddUpdate(input);
            return Ok(result);
        }

        [HttpDelete("CallTypeDelete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OutputDTO<bool>))]
        public async Task<IActionResult> CallTypeDelete(int id)
        {
            var result = await _adminServices.CallTypeDelete(id);
            return Ok(result);
        }

        [HttpGet("CallTypeGet")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OutputDTO<List<CallTypeDto>>))]
        public async Task<IActionResult> CallTypeGet()
        {
            var result = await _adminServices.CallTypeGet();
            return Ok(result);
        }

        #endregion


        #region Contact Type

        [HttpPost("ContactTypeAddUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OutputDTO<bool>))]
        public async Task<IActionResult> ContactTypeAddUpdate([FromBody] ContactTypeDto input)
        {
            var result = await _adminServices.ContactTypeAddUpdate(input);
            return Ok(result);
        }

        [HttpDelete("ContactTypeDelete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OutputDTO<bool>))]
        public async Task<IActionResult> ContactTypeDelete(int id)
        {
            var result = await _adminServices.ContactTypeDelete(id);
            return Ok(result);
        }

        [HttpGet("ContactTypeGet")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OutputDTO<List<ContactTypeDto>>))]
        public async Task<IActionResult> ContactTypeGet()
        {
            var result = await _adminServices.ContactTypeGet();
            return Ok(result);
        }

        #endregion


        #region Skill Master

        [HttpPost("SkillMasterAddUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OutputDTO<bool>))]
        public async Task<IActionResult> SkillMasterAddUpdate([FromBody] SkillMasterDto input)
        {
            var result = await _adminServices.SkillMasterAddUpdate(input);
            return Ok(result);
        }

        [HttpDelete("SkillMasterDelete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OutputDTO<bool>))]
        public async Task<IActionResult> SkillMasterDelete(int id)
        {
            var result = await _adminServices.SkillMasterDelete(id);
            return Ok(result);
        }

        [HttpGet("SkillMasterGet")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OutputDTO<List<SkillMasterDto>>))]
        public async Task<IActionResult> SkillMasterGet()
        {
            var result = await _adminServices.SkillMasterGet();
            return Ok(result);
        }

        #endregion

        #region User Master


        [HttpPost("UserAddUpdate")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OutputDTO<bool>))]
        public async Task<IActionResult> UserAddUpdate(UserMasterDto user)
        {
            var result = await _userMaster.UserAddUpdate(user);
            return Ok(result);
        }

        [HttpDelete("UserDelete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OutputDTO<bool>))]
        public async Task<IActionResult> UserDelete(string userId)
        {
            var result = await _userMaster.UserDelete(userId);
            return Ok(result);
        }

        [HttpGet("UsersListGet")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OutputDTO<List<UserMasterDto>>))]
        public async Task<IActionResult> UsersListGet()
        {
            var result = await _userMaster.UsersListGet();
            return Ok(result);
        }

        [HttpGet("UserProfileGet")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OutputDTO<UserMasterDto>))]
        public async Task<IActionResult> UserProfileGet(string userId)
        {
            var result = await _userMaster.UserProfileGet(userId);
            return Ok(result);
        }

        #endregion


        #region Document Upload



        [Consumes("multipart/form-data")]
        [HttpPost("DocumentAddUpdate")]
        public async Task<IActionResult> DocumentAddUpdate([FromForm] DocumentDto input)
        {
            var result = await _documentUpload.DocumentAddUpdate(input);
            return Ok(result);
        }


        [HttpDelete("DocumentDelete")]
        public async Task<IActionResult> DocumentDelete(int id)
        {
            var result = await _documentUpload.DocumentDelete(id);
            return Ok(result);
        }


        [HttpGet("DocumentsListGet")]
        public async Task<IActionResult> DocumentsListGet(int clientId, int managerId, int consultantId, int requisitionId, string source)
        {
            var result = await _documentUpload.DocumentsListGet(clientId, managerId, consultantId, requisitionId, source);
            return Ok(result);
        }


        #endregion


        #region Call Record

        [HttpPost("AddOrUpdateCallRecord")]
        public async Task<IActionResult> AddOrUpdateCallRecord([FromBody] CallRecordDto input)
        {
            var result = await _callRecord.AddOrUpdateCallRecord(input);
            return Ok(result);
        }

        [HttpDelete("DeleteCallRecord")]
        public async Task<IActionResult> DeleteCallRecord(int id)
        {
            var result = await _callRecord.DeleteCallRecord(id);
            return Ok(result);
        }

        [HttpGet("GetCallRecords")]
        public async Task<IActionResult> GetCallRecords(int leadsId, int managerId, int consultantId)
        {
            var result = await _callRecord.CallRecordsListGet(leadsId, managerId, consultantId);
            return Ok(result);
        }

        #endregion



        #region Support Ticket


        [Consumes("multipart/form-data")]
        [HttpPost("SupportTicketAddUpdate")]
        public async Task<IActionResult> SupportTicketAddUpdate([FromForm] SupportTicketDto input)
        {
            var result = await _adminServices.SupportTicketAddUpdate(input);
            return Ok(result);
        }

        [HttpDelete("SupportTicketDelete")]
        public async Task<IActionResult> SupportTicketDelete(int id)
        {
            var result = await _adminServices.SupportTicketDelete(id);
            return Ok(result);
        }

        [HttpGet("SupportTicketListGet")]
        public async Task<IActionResult> SupportTicketListGet()
        {
            var result = await _adminServices.SupportTicketListGet();
            return Ok(result);
        }


        #endregion


        #region Dashboard

        [HttpGet("GetAllDashboardCounts")]
        public async Task<IActionResult> GetAllDashboardCounts()
        {
            var result = await _dashboard.GetAllDashboardCounts();
            return Ok(result);
        }


        [HttpGet("ManagerCallRecordsListGet")]
        public async Task<IActionResult> ManagerCallRecordsListGet(bool? reminder)
        {
            var result = await _dashboard.ManagerCallRecordsListGet(reminder);
            return Ok(result);
        }


        [HttpGet("ConsultantCallRecordsListGet")]
        public async Task<IActionResult> ConsultantCallRecordsListGet(bool? reminder)
        {
            var result = await _dashboard.ConsultantCallRecordsListGet(reminder);
            return Ok(result);
        }


        [HttpGet("RecentRequisitionListGet")]
        public async Task<IActionResult> RecentRequisitionListGet()
        {
            var result = await _dashboard.RecentRequisitionListGet();
            return Ok(result);
        }


        [HttpGet("LeadsListGet")]
        public async Task<IActionResult> LeadsListGet()
        {
            var result = await _dashboard.LeadsListGet();
            return Ok(result);
        }

        [HttpGet("TicketListGet")]
        public async Task<IActionResult> TicketListGet()
        {
            var result = await _dashboard.TicketListGet();
            return Ok(result);
        }

        [HttpGet("GetDashboardInterviewsList")]
        public async Task<IActionResult> GetDashboardInterviewsList()
        {
            var result = await _dashboard.GetDashboardInterviewsList();
            return Ok(result);
        }

        #endregion
    }
}
