using Service.Services.Dashboard.DTOs;
using Utility.OutputData;

namespace Service.Services.Dashboard
{
    public interface IDashboardService
    {
        Task<OutputDTO<DashboardDto>> GetAllDashboardCounts();
        Task<OutputDTO<List<DashboardCallLogsDto>>> ManagerCallRecordsListGet(bool? reminder);
        Task<OutputDTO<List<DashboardCallLogsDto>>> ConsultantCallRecordsListGet(bool? reminder);
        Task<OutputDTO<List<DashboardRecentRequisitionDto>>> RecentRequisitionListGet();
        Task<OutputDTO<List<DashboardLeadsDto>>> LeadsListGet();
        Task<OutputDTO<List<DashboardSupportTicketDto>>> TicketListGet();
        Task<OutputDTO<List<DashboardInterviewsDto>>> GetDashboardInterviewsList();
    }
}
