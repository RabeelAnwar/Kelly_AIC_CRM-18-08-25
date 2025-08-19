using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;
using Service.Services.Consultant.DTOs;
using Utility.OutputData;

namespace Service.Services.Consultant
{
    public interface IConsultantService
    {
        Task<OutputDTO<ConsultantDto>> ConsultantAddUpdate(ConsultantDto input);
        Task<OutputDTO<bool>> ConsultantDelete(int id);
        Task<OutputDTO<List<ConsultantDto>>> ConsultantsListGet();
        Task<OutputDTO<string>> ExtractTextAsync(string fileUrl);
        Task<OutputDTO<ConsultantDto>> SingleConsultantGet(int id);
        Task<OutputDTO<bool>> DeleteConsultantFileAsync(int id);
        Task<OutputDTO<List<ConsultantActivityDto>>> SearchConsultantsForActivity(string name);
        Task<OutputDTO<ConsultantActivityDto>> ConsultantActivityAddUpdate(ConsultantActivityDto input);
        Task<OutputDTO<bool>> ConsultantActivityDelete(int id);
        Task<OutputDTO<List<ConsultantActivityDto>>> InterviewProcessConsultantsList(int? requisitionId);
        Task<OutputDTO<ConsultantActivityDto>> InterviewProcessConsultantSingle(int id);


        Task<OutputDTO<bool>> InterviewAddUpdate(ConsultantInterviewDto input);
        Task<OutputDTO<bool>> InterviewDelete(int id);
        Task<OutputDTO<List<ConsultantInterviewDto>>> InterviewListGet();

        Task<OutputDTO<bool>> ReferenceAddUpdate(ConsultantReferenceDto input);
        Task<OutputDTO<bool>> ReferenceDelete(int id);
        Task<OutputDTO<List<ConsultantReferenceDto>>> ReferenceListGet();


        Task<OutputDTO<ConsultantInterviewProcessDto>> InterviewProcessAddUpdate(ConsultantInterviewProcessDto input);
        Task<OutputDTO<bool>> InterviewProcessDelete(int id);


    }
}
