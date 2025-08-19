using DataAccess.Entities;
using Service.Services.Admin.CallType;
using Service.Services.Admin.ContactType;
using Service.Services.Admin.Department;
using Service.Services.Admin.DocumentType;
using Service.Services.Admin.SkillMaster;
using Service.Services.User.UserMaster;
using Service.Services.Auth.DTOs;
using Service.Services.Client.DTOs;
using Service.Services.Company.DTOs;
using Service.Services.Consultant.DTOs;
using Service.Services.Lead.DTOs;
using Service.Services.CallRecord.DTOs;
using Service.Services.DocumentUpload.DTOs;
using Service.Services.Requisition.DTOs;
using Service.Services.Admin.SupportTicket;
using Service.Services.UserLogs.DTOs;

namespace Service.AutoMappers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            CreateMap<CompanyInput, Company>();
            CreateMap<Company, CompanyInput>();
            CreateMap<DepartmentDto, Department>();
            CreateMap<Department, DepartmentDto>();
            CreateMap<DocumentType, DocumentTypeDto>();
            CreateMap<DocumentTypeDto, DocumentType>();
            CreateMap<CallType, CallTypeDto>();
            CreateMap<CallTypeDto, CallType>();
            CreateMap<ContactType, ContactTypeDto>();
            CreateMap<ContactTypeDto, ContactType>();
            CreateMap<SkillMaster, SkillMasterDto>();
            CreateMap<SkillMasterDto, SkillMaster>();
            CreateMap<LeadInput, Lead>();
            CreateMap<Lead, LeadInput>();
            CreateMap<Client, ClientInput>();
            CreateMap<ClientInput, Client>();
            CreateMap<Consultant, ConsultantDto>();
            CreateMap<ConsultantDto, Consultant>();
            CreateMap<ClientManager, ClientManagerDto>();
            CreateMap<ClientManagerDto, ClientManager>();
            CreateMap<ClientPipeline, ClientPipelineDto>();
            CreateMap<ClientPipelineDto, ClientPipeline>();
            CreateMap<ConsultantInterviewDto, ConsultantInterview>();
            CreateMap<ConsultantInterview, ConsultantInterviewDto>();
            CreateMap<CallRecord, CallRecordDto>();
            CreateMap<CallRecordDto, CallRecord>();
            CreateMap<Lead, LeadListDto>();
            CreateMap<DocumentUpload, DocumentDto>();
            CreateMap<DocumentDto, DocumentUpload>();
            CreateMap<ClientRequisition, RequisitionDto>();
            CreateMap<RequisitionDto, ClientRequisition>();
            CreateMap<ConsultantActivity, ConsultantActivityDto>();
            CreateMap<ConsultantActivityDto, ConsultantActivity>();
            CreateMap<ConsultantInterviewProcess, ConsultantInterviewProcessDto>();
            CreateMap<ConsultantInterviewProcessDto, ConsultantInterviewProcess>();
            CreateMap<SupportTicketDto, SupportTicket>();
            CreateMap<SupportTicket, SupportTicketDto>();
            CreateMap<UsersLogDto, UsersLog>();
            CreateMap<UsersLog, UsersLogDto>();


            /////////////////////////////////////////////////////////////////////////////////////////////
            CreateMap<UserMasterDto, User>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.PersonalEmail))
            .ForMember(dest => dest.NormalizedEmail, opt => opt.MapFrom(src => src.PersonalEmail.ToUpper()))
            .ForMember(dest => dest.NormalizedUserName, opt => opt.MapFrom(src => src.UserName.ToUpper()))
            .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<User, UserMasterDto>()
                .ForMember(dest => dest.PersonalEmail, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Password, opt => opt.Ignore());

        }
    }
}
