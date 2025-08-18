using System;
using AIC_CRM_API.MiddleWares;
using Repository.GenericRepository;
using Repository.UnitOfWorks;
using Service.AutoMappers;
using Service.EntityAudit;
using Service.FileUpload;
using Service.Services.Admin;
using Service.Services.Auth;
using Service.Services.CallRecord;
using Service.Services.Client;
using Service.Services.Company;
using Service.Services.Consultant;
using Service.Services.Dashboard;
using Service.Services.DocumentUpload;
using Service.Services.Lead;
using Service.Services.Leads;
using Service.Services.Reports;
using Service.Services.Requisition;
using Service.Services.User;
using Service.Sessions;

namespace AIC_CRM_API.Installers
{
    public class ServicesInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ExceptionHandling>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddAutoMapper(typeof(AutoMapperProfile));
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ISessionData, SessionData>();
            services.AddScoped<AuditSaveChangesInterceptor>();
            services.AddScoped<IAdminServices, AdminServices>();
            services.AddScoped<ILeadService, LeadService>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IConsultantService, ConsultantService>();
            services.AddScoped<IUserMasterService, UserMasterService>();
            services.AddScoped<IFileUpload, FileUpload>();
            services.AddScoped<IDocumentUploadService, DocumentUploadService>();
            services.AddScoped<ICallRecordService, CallRecordService>();
            services.AddScoped<IRequisitionService, RequisitionService>();
            services.AddScoped<ICrmReports, CrmReports>();
            services.AddScoped<IDashboardService, DashboardService>();
        }
    }
}
