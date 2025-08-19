using Service.Services.Consultant.DTOs;
using Service.Services.Requisition.DTOs;
using Utility.OutputData;

namespace Service.Services.Requisition
{
    public interface IRequisitionService
    {
        Task<OutputDTO<RequisitionDto>> ClientRequisitionAddUpdate(RequisitionDto input);
        Task<OutputDTO<bool>> ClientRequisitionDelete(int id);
        Task<OutputDTO<bool>> ClientRequisitionStatusUpdate(RequisitionDto input);
        Task<OutputDTO<List<RequisitionDto>>> ClientRequisitionsListGet();
        Task<OutputDTO<List<RequisitionDto>>> ClientOpenRequisitionsListGet();
        Task<OutputDTO<List<RequisitionDto>>> ClientRequisitionsListGetByClientId(int clientId);
        Task<OutputDTO<RequisitionDto>> ClientRequisitionGet(int id);
        Task<OutputDTO<List<RequisitionDto>>> ClientRequisitionGetByClientId(int id);
    }
}
