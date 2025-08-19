using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Services.CallRecord.DTOs;
using Utility.OutputData;

namespace Service.Services.CallRecord
{
    public interface ICallRecordService
    {
        Task<OutputDTO<bool>> AddOrUpdateCallRecord(CallRecordDto input);
        Task<OutputDTO<bool>> DeleteCallRecord(int id);
        Task<OutputDTO<List<CallRecordDto>>> CallRecordsListGet(int leadsId, int managerId, int consultantId);
    }
}
