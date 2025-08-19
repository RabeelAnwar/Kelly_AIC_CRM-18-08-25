using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Services.Consultant.DTOs;
using Service.Services.Reports.DTOs;
using Utility.OutputData;

namespace Service.Services.Reports
{
    public interface ICrmReports
    {
        Task<OutputDTO<List<RecruitersRptDto>>> AllRecruitersRptGet(RecruitersRptDto input);
        Task<OutputDTO<List<RecruitersRptDto>>> AllRecruitersRequisitionRptGet(RecruitersRptDto input);


        Task<OutputDTO<RecruitersRptDto>> IndividualReportRptGet(RecruitersRptDto input);
        Task<OutputDTO<RecruitersRptDto>> IndividualRequisitionRptGet(RecruitersRptDto input);


        Task<OutputDTO<MasterOfficeRptDto>> OfficeMasterRptGet(MasterOfficeRptDto input);

        Task<OutputDTO<List<AuditRptDto>>> AuditRptGet(AuditRptDto input);
    }
}
