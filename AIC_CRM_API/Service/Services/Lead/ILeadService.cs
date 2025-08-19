using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Services.Company.DTOs;
using Service.Services.Lead.DTOs;
using Utility.OutputData;

namespace Service.Services.Lead
{
    public interface ILeadService
    {
        Task<OutputDTO<bool>> LeadAddUpdate(LeadInput input);
        Task<OutputDTO<bool>> LeadDelete(int id);
        Task<OutputDTO<List<LeadInput>>> LeadsListGet();
        Task<OutputDTO<List<LeadListDto>>> DetailedLeadsListGet();
        Task<OutputDTO<LeadInput>> SingleLeadGet();

    }
}
