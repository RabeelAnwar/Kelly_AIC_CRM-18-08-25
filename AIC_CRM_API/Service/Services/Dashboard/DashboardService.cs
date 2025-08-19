using System;
using DataAccess.Entities;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using Service.Converter;
using Service.Services.CallRecord.DTOs;
using Service.Services.Dashboard.DTOs;
using Service.Services.Reports.DTOs;
using Service.Sessions;
using Utility.OutputData;

namespace Service.Services.Dashboard
{
    public class DashboardService : IDashboardService
    {
        private readonly MainContext _context;
        private readonly IMapper _mapper;
        private readonly ISessionData _session;

        public DashboardService(MainContext context, IMapper mapper, ISessionData sessionData)
        {
            _context = context;
            _mapper = mapper;
            _session = sessionData;
        }

        public async Task<OutputDTO<DashboardDto>> GetAllDashboardCounts()
        {
            try
            {
                var tenantId = _session.TenantId;
                var userId = _session.UserId;

                var since = DateTimeOffset.UtcNow.AddHours(-48);
                var todayDate = DateTime.UtcNow.Date;

                int callRecordsClient = _context.CallRecords
                    .Where(x => x.TenantId == tenantId
                                && x.CreatorUserId == userId
                                && x.CreationTime >= since
                                && x.ClientId.HasValue || x.ManagerId.HasValue)
                    .Count();

                int callRecordsConsultant = _context.CallRecords
                    .Where(x => x.TenantId == tenantId
                                && x.CreatorUserId == userId
                                && x.CreationTime >= since
                                && x.ConsultantId.HasValue)
                    .Count();

                int recentRequisitions = _context.ClientRequisitions
                    .Where(x => x.TenantId == tenantId
                                && x.CreatorUserId == userId
                                && x.CreationTime >= since)
                    .Count();

                int leads = _context.Leads
                    .Where(x => x.TenantId == tenantId
                                && x.CreatorUserId == userId
                                && x.CreationTime >= since)
                    .Count();


                int reminderCalls = _context.CallRecords
                                        .Where(x => x.RemindStatus == true && x.TenantId == tenantId)
                                        .Count();

                int supportTickets = _context.SupportTickets.Where(x => x.TenantId == tenantId).Count();
                int activities = _context.UsersLogs.Where(x => x.TenantId == tenantId).Count();



                int totalCalls = _context.CallRecords
                                       .Where(x => x.CreationTime.Date == todayDate && x.TenantId == tenantId)
                                       .Count();


                int totalReq = _context.ClientRequisitions
                                       .Where(x => x.CreationTime.Date == todayDate && x.TenantId == tenantId)
                                       .Count();

                int totalSubmitals = _context.ConsultantActivities
                                      .Where(x => x.CreationTime.Date == todayDate && x.TenantId == tenantId)
                                      .Count();

                var result = new DashboardDto
                {
                    RecentClientCallRecords = callRecordsClient,
                    RecentConsultantCallRecords = callRecordsConsultant,
                    RecentRequisitions = recentRequisitions,
                    MyLeads = leads,
                    ReminderCalls = reminderCalls,
                    SupportTickets = supportTickets,
                    Activities = activities,

                    TodayCallRecords = totalCalls,
                    TodayRequisitionsTotal = totalReq,
                    TodaySubmitalsTotal = totalSubmitals
                };

                return OutputHandler.Handler((int)ResponseType.GET, result, "Dashboard", 0);
            }
            catch (Exception)
            {
                throw;
            }
        }



        public async Task<OutputDTO<List<DashboardCallLogsDto>>> ManagerCallRecordsListGet(bool? reminder)
        {
            try
            {
                var tenantId = _session.TenantId;
                var since = DateTimeOffset.UtcNow.AddHours(-48);

                if (reminder == true)
                {
                    var resultReminder = await (from call in _context.CallRecords

                                                join user in _context.Users on call.CreatorUserId equals user.Id into userGroup
                                                from creator in userGroup.DefaultIfEmpty()

                                                join m in _context.ClientManagers on call.ManagerId equals m.Id into manager
                                                from m in manager.DefaultIfEmpty()

                                                where call.TenantId == tenantId && call.ManagerId > 0 && call.RemindStatus == true
                                                select new DashboardCallLogsDto
                                                {
                                                    Id = call.Id,
                                                    Datetime = call.CreationTime,
                                                    ManagerName = m.FirstName + " " + m.MiddleName + " " + m.LastName,
                                                    SpokeWith = creator.FirstName + " " + creator.MiddleName + " " + creator.LastName,
                                                    CallRecord = call.Record
                                                }).OrderByDescending(x => x.Id).ToListAsync();


                    return OutputHandler.Handler((int)ResponseType.GET, resultReminder, "Call Records", resultReminder.Count);

                }

                var result = await (from call in _context.CallRecords

                                    join user in _context.Users on call.CreatorUserId equals user.Id into userGroup
                                    from creator in userGroup.DefaultIfEmpty()

                                    join m in _context.ClientManagers on call.ManagerId equals m.Id into manager
                                    from m in manager.DefaultIfEmpty()

                                    where call.TenantId == tenantId && call.ManagerId > 0 && call.CreationTime >= since
                                    select new DashboardCallLogsDto
                                    {
                                        Id = call.Id,
                                        Datetime = call.CreationTime,
                                        ManagerName = m.FirstName + " " + m.MiddleName + " " + m.LastName,
                                        SpokeWith = creator.FirstName + " " + creator.MiddleName + " " + creator.LastName,
                                        CallRecord = call.Record
                                    }).OrderByDescending(x => x.Id).ToListAsync();


                return OutputHandler.Handler((int)ResponseType.GET, result, "Call Records", result.Count);
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }



        public async Task<OutputDTO<List<DashboardCallLogsDto>>> ConsultantCallRecordsListGet(bool? reminder)
        {
            try
            {
                var tenantId = _session.TenantId;
                var since = DateTimeOffset.UtcNow.AddHours(-48);

                if (reminder == true)
                {
                    var resultReminder = await (from call in _context.CallRecords

                                                join user in _context.Users on call.CreatorUserId equals user.Id into userGroup
                                                from creator in userGroup.DefaultIfEmpty()

                                                join c in _context.Consultants on call.ConsultantId equals c.Id into consultant
                                                from c in consultant.DefaultIfEmpty()

                                                where call.TenantId == tenantId && call.ConsultantId > 0 && call.RemindStatus == true
                                                select new DashboardCallLogsDto
                                                {
                                                    Id = call.Id,
                                                    Datetime = call.CreationTime,
                                                    ConsultantName = c.FirstName + " " + c.MiddleName + " " + c.LastName,
                                                    SpokeWith = creator.FirstName + " " + creator.MiddleName + " " + creator.LastName,
                                                    CallRecord = call.Record
                                                }).OrderByDescending(x => x.Id).ToListAsync();


                    return OutputHandler.Handler((int)ResponseType.GET, resultReminder, "Call Records", resultReminder.Count);
                }

                var result = await (from call in _context.CallRecords

                                    join user in _context.Users on call.CreatorUserId equals user.Id into userGroup
                                    from creator in userGroup.DefaultIfEmpty()

                                    join c in _context.Consultants on call.ConsultantId equals c.Id into consultant
                                    from c in consultant.DefaultIfEmpty()

                                    where call.TenantId == tenantId && call.ConsultantId > 0 && call.CreationTime >= since
                                    select new DashboardCallLogsDto
                                    {
                                        Id = call.Id,
                                        Datetime = call.CreationTime,
                                        ConsultantName = c.FirstName + " " + c.MiddleName + " " + c.LastName,
                                        SpokeWith = creator.FirstName + " " + creator.MiddleName + " " + creator.LastName,
                                        CallRecord = call.Record
                                    }).OrderByDescending(x => x.Id).ToListAsync();


                return OutputHandler.Handler((int)ResponseType.GET, result, "Call Records", result.Count);
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }


        public async Task<OutputDTO<List<DashboardRecentRequisitionDto>>> RecentRequisitionListGet()
        {
            try
            {
                var tenantId = _session.TenantId;
                var since = DateTimeOffset.UtcNow.AddHours(-48);

                var result = await (from r in _context.ClientRequisitions

                                    join m in _context.ClientManagers on r.ManagerId equals m.Id into manager
                                    from m in manager.DefaultIfEmpty()

                                    join c in _context.Clients on r.ClientId equals c.Id into client
                                    from c in client.DefaultIfEmpty()

                                    where r.TenantId == tenantId && r.CreationTime >= since
                                    select new DashboardRecentRequisitionDto
                                    {
                                        Id = r.Id,
                                        Requisition = r.JobTitle,
                                        Type = r.RequisitionType,
                                        ClientName = c.ClientName,
                                        ManagerName = m.FirstName + " " + m.MiddleName + " " + m.LastName,
                                        Buzzwords = "",
                                        Duration = r.Duration + " " + r.DurationTypes,
                                        BillRate = r.BillRate,
                                        CreatedDatetime = r.CreationTime
                                    }).OrderByDescending(x => x.Id).ToListAsync();


                return OutputHandler.Handler((int)ResponseType.GET, result, "Call Records", result.Count);
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }


        public async Task<OutputDTO<List<DashboardLeadsDto>>> LeadsListGet()
        {
            try
            {
                var tenantId = _session.TenantId;

                var result = await (from l in _context.Leads

                                    join m in _context.ClientManagers on l.ManagerId equals m.Id into manager
                                    from m in manager.DefaultIfEmpty()

                                    join c in _context.Clients on l.ClientId equals c.Id into client
                                    from c in client.DefaultIfEmpty()

                                    where l.TenantId == tenantId
                                    select new DashboardLeadsDto
                                    {
                                        Id = l.Id,
                                        ClientName = c.ClientName,
                                        Date = l.ReminderDateTime,
                                        ManagerName = m.FirstName + " " + m.MiddleName + " " + m.LastName,
                                        ManagerContact = m.OfficePhone + "-" + m.OfficePhoneExt,
                                        Amount = l.ApproximateAmount,
                                        Details = l.LeadInformation,
                                    }).OrderByDescending(x => x.Id).ToListAsync();


                return OutputHandler.Handler((int)ResponseType.GET, result, "Call Records", result.Count);
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }


        public async Task<OutputDTO<List<DashboardSupportTicketDto>>> TicketListGet()
        {
            try
            {
                var tenantId = _session.TenantId;

                var result = await (from t in _context.SupportTickets

                                    join u in _context.Users on t.CreatorUserId equals u.Id into users
                                    from u in users.DefaultIfEmpty()

                                    where t.TenantId == tenantId
                                    select new DashboardSupportTicketDto
                                    {
                                        Id = t.Id,
                                        Datetime = t.CreationTime,
                                        Type = t.Type,
                                        Status = t.Status,
                                        Priority = t.Priority,
                                        RaisedBy = u.FirstName + " " + u.MiddleName + " " + u.LastName,
                                        Subject = t.Subject,
                                        Description = t.Description,
                                    }).OrderByDescending(x => x.Id).ToListAsync();


                return OutputHandler.Handler((int)ResponseType.GET, result, "Call Records", result.Count);
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }




        public async Task<OutputDTO<List<DashboardInterviewsDto>>> GetDashboardInterviewsList()
        {
            try
            {
                var tenantId = _session.TenantId;

                var reqInterviews = await (
                    from r in _context.ClientRequisitions

                    join c in _context.Clients on r.ClientId equals c.Id into clients
                    from c in clients.DefaultIfEmpty()

                    join a in _context.ConsultantActivities on r.Id equals a.RequisitionId into activity
                    from a in activity.DefaultIfEmpty()

                    join ips in _context.ConsultantInterviewProcesses on a.Id equals ips.ConsultantActivityId into intProcess
                    from ips in intProcess.DefaultIfEmpty()

                    join cl in _context.Consultants on a.ConsultantId equals cl.Id into consultants
                    from cl in consultants.DefaultIfEmpty()

                    where r.TenantId == tenantId && ips.ExpectedStartDate.HasValue && ips.Salary > 0

                    select new DashboardInterviewsDto
                    {
                        ConsultantName = cl.FirstName + " " + cl.MiddleName + " " + cl.LastName,
                        ConsultantId = cl.Id,
                        Client = c.ClientName,
                        ClientId = c.Id,
                        ReqNo = r.ClientReqNumber,
                        ReqId = r.Id,
                        BillRate = a.BillRate,
                        PayRate = a.PayRate,
                        InterviewTime = ips.CreationTime,
                        SubmissionDate = a.CreationTime,
                        Notes = ips.Notes,
                    }).ToListAsync();


                return OutputHandler.Handler((int)ResponseType.GET, reqInterviews, "Interviews", reqInterviews.Count);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
