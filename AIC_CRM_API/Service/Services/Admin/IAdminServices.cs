using Service.Services.Admin.CallType;
using Service.Services.Admin.ContactType;
using Service.Services.Admin.Department;
using Service.Services.Admin.DocumentType;
using Service.Services.Admin.SkillMaster;
using Service.Services.Admin.SupportTicket;
using Service.Services.Consultant.DTOs;
using Utility.OutputData;

namespace Service.Services.Admin
{
    public interface IAdminServices
    {

        #region Department

        Task<OutputDTO<bool>> DepartmentAddUpdate(DepartmentDto input);
        Task<OutputDTO<bool>> DepartmentDelete(int id);
        Task<OutputDTO<List<DepartmentDto>>> DepartmentGet();

        #endregion

        #region Document Type

        Task<OutputDTO<bool>> DocumentTypeAddUpdate(DocumentTypeDto input);
        Task<OutputDTO<bool>> DocumentTypeDelete(int id);
        Task<OutputDTO<List<DocumentTypeDto>>> DocumentTypeGet();

        #endregion

        #region Call Type


        Task<OutputDTO<bool>> CallTypeAddUpdate(CallTypeDto input);
        Task<OutputDTO<bool>> CallTypeDelete(int id);
        Task<OutputDTO<List<CallTypeDto>>> CallTypeGet();

        #endregion

        #region Contact Type

        Task<OutputDTO<bool>> ContactTypeAddUpdate(ContactTypeDto input);
        Task<OutputDTO<bool>> ContactTypeDelete(int id);
        Task<OutputDTO<List<ContactTypeDto>>> ContactTypeGet();


        #endregion

        #region Skill Master

        Task<OutputDTO<bool>> SkillMasterAddUpdate(SkillMasterDto input);
        Task<OutputDTO<bool>> SkillMasterDelete(int id);
        Task<OutputDTO<List<SkillMasterDto>>> SkillMasterGet();

        #endregion


        #region Support Ticket

        Task<OutputDTO<SupportTicketDto>> SupportTicketAddUpdate(SupportTicketDto input);
        Task<OutputDTO<bool>> SupportTicketDelete(int id);
        Task<OutputDTO<List<SupportTicketDto>>> SupportTicketListGet();

        #endregion

    }
}