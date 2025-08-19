using System.Net.NetworkInformation;
using DataAccess.Entities;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using Service.Services.Reports.DTOs;
using Service.Services.UserLogs.DTOs;
using Service.Sessions;
using Utility.OutputData;

namespace Service.Services.Reports
{
    public class CrmReports : ICrmReports
    {

        private readonly MainContext _context;
        private readonly IMapper _mapper;
        private readonly ISessionData _session;

        public CrmReports(MainContext context, IMapper mapper, ISessionData sessionData)
        {
            _context = context;
            _mapper = mapper;
            _session = sessionData;
        }

        public async Task<OutputDTO<List<RecruitersRptDto>>> AllRecruitersRptGet(RecruitersRptDto input)
        {
            try
            {
                var tenantId = _session.TenantId;

                var fromDate = input.FromDate?.Date ?? DateTime.Today;
                var toDate = input.ToDate?.Date.AddDays(1).AddTicks(-1) ?? DateTime.Today.AddDays(1).AddTicks(-1);

                var users = await _context.Users
                    .Where(x => x.TenantId == tenantId)
                    .ToListAsync();

                var result = users.Select(user => new RecruitersRptDto
                {
                    Recruiter = user.FirstName = " " + user.LastName,

                    Submittals = _context.ConsultantActivities.Count(x =>
                       x.CreatorUserId == user.Id &&
                       x.TenantId == tenantId &&
                       x.CreationTime.Date >= fromDate.Date &&
                       x.CreationTime.Date <= toDate.Date),

                    Interviews = _context.ConsultantInterviewProcesses.Count(x =>
                         x.CreatorUserId == user.Id &&
                         x.TenantId == tenantId &&
                         x.CreationTime.Date >= fromDate.Date &&
                         x.CreationTime.Date <= toDate.Date &&
                         (x.Salary == 0 && x.HourlyRate == 0)),

                    Starts = _context.ConsultantInterviewProcesses.Count(x =>
                        x.CreatorUserId == user.Id &&
                        x.TenantId == tenantId &&
                        x.Salary > 0 && x.HourlyRate > 0 &&
                        x.ExpectedStartDate.Value.Date >= fromDate.Date &&
                        x.ExpectedStartDate.Value.Date <= toDate.Date),

                    ConsultantsAdded = _context.Consultants.Count(x =>
                        x.CreatorUserId == user.Id &&
                        x.TenantId == tenantId &&
                        x.CreationTime.Date >= fromDate.Date &&
                        x.CreationTime.Date <= toDate.Date),

                    CallLogsAdded = _context.CallRecords.Count(x =>
                        x.CreatorUserId == user.Id &&
                        x.TenantId == tenantId &&
                        x.CreationTime.Date >= fromDate.Date &&
                        x.CreationTime.Date <= toDate.Date)
                }).ToList();

                return OutputHandler.Handler((int)ResponseType.GET, result, "Recruiter Report", result.Count);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<OutputDTO<List<RecruitersRptDto>>> AllRecruitersRequisitionRptGet(RecruitersRptDto input)
        {
            try
            {
                var tenantId = _session.TenantId;

                var fromDate = input.FromDate?.Date ?? DateTime.Today;
                var toDate = input.ToDate?.Date ?? DateTime.Today;

                var data = await (
                    from r in _context.ClientRequisitions
                    join u in _context.Users on r.CreatorUserId equals u.Id
                    join c in _context.Clients on r.ClientId equals c.Id into clients
                    from c in clients.DefaultIfEmpty()
                    where r.TenantId == tenantId
                    select new
                    {
                        RecruiterId = u.Id,
                        RecruiterName = u.FirstName + " " + u.LastName,
                        RequisitionId = r.Id,
                        ClientName = c.ClientName,
                        JobTitle = r.JobTitle,

                        SubmittalsCount = _context.ConsultantActivities.Count(ca =>
                            ca.RequisitionId == r.Id &&
                            ca.CreatorUserId == u.Id &&
                            ca.TenantId == tenantId &&
                            ca.CreationTime.Date >= fromDate.Date &&
                            ca.CreationTime.Date <= toDate.Date
                        ),

                        InterviewsCount = _context.ConsultantInterviewProcesses.Count(cip =>
                            cip.CreatorUserId == u.Id &&
                            cip.TenantId == tenantId &&
                            cip.CreationTime.Date >= fromDate.Date &&
                            cip.CreationTime.Date <= toDate.Date &&
                            (cip.Salary == 0 &&
                            cip.HourlyRate == 0) &&
                            _context.ConsultantActivities.Any(ca2 => ca2.Id == cip.ConsultantActivityId && ca2.RequisitionId == r.Id)
                        ),

                        StartsCount = _context.ConsultantInterviewProcesses.Count(cip =>
                            cip.CreatorUserId == u.Id &&
                            cip.TenantId == tenantId &&
                            cip.Salary > 0 && cip.HourlyRate > 0 &&
                            cip.ExpectedStartDate.Value.Date >= fromDate.Date &&
                            cip.ExpectedStartDate.Value.Date <= toDate.Date
                            &&
                            _context.ConsultantActivities.Any(ca3 => ca3.Id == cip.ConsultantActivityId && ca3.RequisitionId == r.Id)
                        )

                        //                InterviewsCount = (
                        //    from cip in _context.ConsultantInterviewProcesses
                        //    join ca in _context.ConsultantActivities on cip.ConsultantActivityId equals ca.Id
                        //    where cip.CreatorUserId == u.Id &&
                        //          cip.TenantId == tenantId &&
                        //          cip.CreationTime.Date >= fromDate.Date &&
                        //          cip.CreationTime.Date <= toDate.Date &&
                        //          cip.Salary == 0 &&
                        //          cip.HourlyRate == 0 &&
                        //          ca.RequisitionId == r.Id
                        //    select cip
                        //).Count(),

                        //                StartsCount = (
                        //    from cip in _context.ConsultantInterviewProcesses
                        //    join ca in _context.ConsultantActivities on cip.ConsultantActivityId equals ca.Id
                        //    where cip.CreatorUserId == u.Id &&
                        //          cip.TenantId == tenantId &&
                        //          cip.ExpectedStartDate.HasValue &&
                        //          cip.ExpectedStartDate.Value.Date >= fromDate.Date &&
                        //          cip.ExpectedStartDate.Value.Date <= toDate.Date &&
                        //          ca.RequisitionId == r.Id
                        //    select cip
                        //).Count()
                    }).ToListAsync();


                // Group data by recruiter
                var grouped = data.GroupBy(d => new { d.RecruiterId, d.RecruiterName });

                var result = new List<RecruitersRptDto>();

                foreach (var group in grouped)
                {
                    var dto = new RecruitersRptDto
                    {
                        Recruiter = group.Key.RecruiterName,
                        RequisitionsDetails = group.Select(x =>
                            $"Req {x.RequisitionId} | {x.ClientName} - {x.JobTitle} | Subs: {x.SubmittalsCount}; Interviews: {x.InterviewsCount}; Starts: {x.StartsCount}"
                        ).ToList()
                    };

                    result.Add(dto);
                }

                return OutputHandler.Handler((int)ResponseType.GET, result, "Recruiter Report", result.Count);
            }
            catch (Exception ex)
            {
                throw;
            }
        }




        public async Task<OutputDTO<RecruitersRptDto>> IndividualReportRptGet(RecruitersRptDto input)
        {
            try
            {
                var tenantId = _session.TenantId;
                var userId = _session.UserId;

                var fromDate = input.FromDate?.Date ?? DateTime.Today;
                var toDate = input.ToDate?.Date.AddDays(1).AddTicks(-1) ?? DateTime.Today.AddDays(1).AddTicks(-1);

                var user = await _context.Users
                    .Where(x => x.TenantId == tenantId && x.Id == userId)
                    .FirstOrDefaultAsync();

                var result = new RecruitersRptDto
                {
                    Recruiter = user.FirstName = " " + user.LastName,

                    Submittals = _context.ConsultantActivities.Count(x =>
                       x.CreatorUserId == user.Id &&
                       x.TenantId == tenantId &&
                       x.CreationTime.Date >= fromDate.Date &&
                       x.CreationTime.Date <= toDate.Date),

                    Interviews = _context.ConsultantInterviewProcesses.Count(x =>
                         x.CreatorUserId == user.Id &&
                         x.TenantId == tenantId &&
                         x.CreationTime.Date >= fromDate.Date &&
                         x.CreationTime.Date <= toDate.Date &&
                         (x.Salary == 0 && x.HourlyRate == 0)),

                    Starts = _context.ConsultantInterviewProcesses.Count(x =>
                        x.CreatorUserId == user.Id &&
                        x.TenantId == tenantId &&
                        x.Salary > 0 && x.HourlyRate > 0 &&
                        x.ExpectedStartDate.Value.Date >= fromDate.Date &&
                        x.ExpectedStartDate.Value.Date <= toDate.Date),

                    ConsultantsAdded = _context.Consultants.Count(x =>
                        x.CreatorUserId == user.Id &&
                        x.TenantId == tenantId &&
                        x.CreationTime.Date >= fromDate.Date &&
                        x.CreationTime.Date <= toDate.Date),

                    CallLogsAdded = _context.CallRecords.Count(x =>
                        x.CreatorUserId == user.Id &&
                        x.TenantId == tenantId &&
                        x.CreationTime.Date >= fromDate.Date &&
                        x.CreationTime.Date <= toDate.Date)
                };

                return OutputHandler.Handler((int)ResponseType.GET, result, "Recruiter Report", 0);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<OutputDTO<RecruitersRptDto>> IndividualRequisitionRptGet(RecruitersRptDto input)
        {
            try
            {
                var tenantId = _session.TenantId;
                var userId = _session.UserId;

                var fromDate = input.FromDate?.Date ?? DateTime.Today;
                var toDate = input.ToDate?.Date ?? DateTime.Today;

                var data = await (
                    from r in _context.ClientRequisitions
                    join u in _context.Users on r.CreatorUserId equals u.Id
                    join c in _context.Clients on r.ClientId equals c.Id into clients
                    from c in clients.DefaultIfEmpty()
                    where r.TenantId == tenantId && r.CreatorUserId == userId
                    select new
                    {
                        RecruiterId = u.Id,
                        RecruiterName = u.FirstName + " " + u.LastName,
                        RequisitionId = r.Id,
                        ClientName = c.ClientName,
                        JobTitle = r.JobTitle,

                        SubmittalsCount = _context.ConsultantActivities.Count(ca =>
                            ca.RequisitionId == r.Id &&
                            ca.CreatorUserId == u.Id &&
                            ca.TenantId == tenantId &&
                            ca.CreationTime.Date >= fromDate.Date &&
                            ca.CreationTime.Date <= toDate.Date
                        ),

                        InterviewsCount = _context.ConsultantInterviewProcesses.Count(cip =>
                            cip.CreatorUserId == u.Id &&
                            cip.TenantId == tenantId &&
                            cip.CreationTime.Date >= fromDate.Date &&
                            cip.CreationTime.Date <= toDate.Date &&
                            (cip.Salary == 0 &&
                            cip.HourlyRate == 0) &&
                            _context.ConsultantActivities.Any(ca2 => ca2.Id == cip.ConsultantActivityId && ca2.RequisitionId == r.Id)
                        ),

                        StartsCount = _context.ConsultantInterviewProcesses.Count(cip =>
                            cip.CreatorUserId == u.Id &&
                            cip.TenantId == tenantId &&
                            cip.Salary > 0 && cip.HourlyRate > 0 &&
                            cip.ExpectedStartDate.Value.Date >= fromDate.Date &&
                            cip.ExpectedStartDate.Value.Date <= toDate.Date
                            &&
                            _context.ConsultantActivities.Any(ca3 => ca3.Id == cip.ConsultantActivityId && ca3.RequisitionId == r.Id)
                        )

                        //                InterviewsCount = (
                        //    from cip in _context.ConsultantInterviewProcesses
                        //    join ca in _context.ConsultantActivities on cip.ConsultantActivityId equals ca.Id
                        //    where cip.CreatorUserId == u.Id &&
                        //          cip.TenantId == tenantId &&
                        //          cip.CreationTime.Date >= fromDate.Date &&
                        //          cip.CreationTime.Date <= toDate.Date &&
                        //          cip.Salary == 0 &&
                        //          cip.HourlyRate == 0 &&
                        //          ca.RequisitionId == r.Id
                        //    select cip
                        //).Count(),

                        //                StartsCount = (
                        //    from cip in _context.ConsultantInterviewProcesses
                        //    join ca in _context.ConsultantActivities on cip.ConsultantActivityId equals ca.Id
                        //    where cip.CreatorUserId == u.Id &&
                        //          cip.TenantId == tenantId &&
                        //          cip.ExpectedStartDate.HasValue &&
                        //          cip.ExpectedStartDate.Value.Date >= fromDate.Date &&
                        //          cip.ExpectedStartDate.Value.Date <= toDate.Date &&
                        //          ca.RequisitionId == r.Id
                        //    select cip
                        //).Count()
                    }).ToListAsync();


                // Group data by recruiter
                var grouped = data.GroupBy(d => new { d.RecruiterId, d.RecruiterName });

                var result = new List<RecruitersRptDto>();

                foreach (var group in grouped)
                {
                    var dto = new RecruitersRptDto
                    {
                        Recruiter = group.Key.RecruiterName,
                        RequisitionsDetails = group.Select(x =>
                            $"Req {x.RequisitionId} | {x.ClientName} - {x.JobTitle} | Subs: {x.SubmittalsCount}; Interviews: {x.InterviewsCount}; Starts: {x.StartsCount}"
                        ).ToList()
                    };

                    result.Add(dto);
                }

                var resp = result.FirstOrDefault();

                return OutputHandler.Handler((int)ResponseType.GET, resp, "Recruiter Report", 0);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<OutputDTO<MasterOfficeRptDto>> OfficeMasterRptGet(MasterOfficeRptDto input)
        {
            try
            {
                var tenantId = _session.TenantId;
                var userId = _session.UserId;

                var fromDate = input.FromDate?.Date ?? DateTime.Today;
                var toDate = input.ToDate?.Date.AddDays(1).AddTicks(-1) ?? DateTime.Today.AddDays(1).AddTicks(-1);

                var reqActivities = await (
                    from r in _context.ClientRequisitions
                    join c in _context.Clients on r.ClientId equals c.Id into clients
                    from c in clients.DefaultIfEmpty()

                    join m in _context.ClientManagers on r.ManagerId equals m.Id into manager
                    from m in manager.DefaultIfEmpty()

                    join u in _context.Users on r.InternalReqCoordinatorId equals u.Id into user
                    from u in user.DefaultIfEmpty()

                    where r.TenantId == tenantId && r.CreationTime.Date >= input.FromDate.Value.Date && r.CreationTime.Date <= input.ToDate.Value.Date
                    select new RequisitionActivity
                    {
                        ClientName = c.ClientName,
                        ClientId = c.Id,
                        ReqName = r.JobTitle,
                        ReqId = r.Id,
                        Duration = r.Duration + " " + r.DurationTypes,
                        BillRate = r.BillRate,
                        NoOfPosition = r.NumberOfPositions,
                        ManagerName = m.FirstName + " " + m.MiddleName + " " + m.LastName,
                        ManagerId = m.Id,
                        CreatedDate = r.CreationTime,

                        Coordinator = u.FirstName + " " + u.MiddleName + " " + u.LastName,

                        Submittals = _context.ConsultantActivities.Count(ca =>
                            ca.RequisitionId == r.Id &&
                            ca.TenantId == tenantId &&
                            ca.CreationTime.Date >= fromDate.Date &&
                            ca.CreationTime.Date <= toDate.Date
                        ),

                        Interviews = _context.ConsultantInterviewProcesses.Count(cip =>
                            cip.TenantId == tenantId &&
                            cip.CreationTime.Date >= fromDate.Date &&
                            cip.CreationTime.Date <= toDate.Date &&
                            (cip.Salary == 0 &&
                            cip.HourlyRate == 0) &&
                            _context.ConsultantActivities.Any(ca2 => ca2.Id == cip.ConsultantActivityId && ca2.RequisitionId == r.Id)
                        ),

                        Starts = _context.ConsultantInterviewProcesses.Count(cip =>
                            cip.TenantId == tenantId &&
                            cip.Salary > 0 && cip.HourlyRate > 0 &&
                            cip.ExpectedStartDate.Value.Date >= fromDate.Date &&
                            cip.ExpectedStartDate.Value.Date <= toDate.Date
                            &&
                            _context.ConsultantActivities.Any(ca3 => ca3.Id == cip.ConsultantActivityId && ca3.RequisitionId == r.Id)
                        )


                    }).ToListAsync();


                var reqInterviews = await (
                    from r in _context.ClientRequisitions

                    join c in _context.Clients on r.ClientId equals c.Id into clients
                    from c in clients.DefaultIfEmpty()

                    join s in _context.Users on r.SalesRepId equals s.Id into user
                    from s in user.DefaultIfEmpty()

                    join rec in _context.Users on r.RecruiterAssignedId[0] equals rec.Id into userRec
                    from rec in userRec.DefaultIfEmpty()

                    join a in _context.ConsultantActivities on r.Id equals a.RequisitionId into activity
                    from a in activity.DefaultIfEmpty()

                    join ips in _context.ConsultantInterviewProcesses on a.Id equals ips.ConsultantActivityId into intProcess
                    from ips in intProcess.DefaultIfEmpty()

                    join cl in _context.Consultants on a.ConsultantId equals cl.Id into consultants
                    from cl in consultants.DefaultIfEmpty()

                    where r.TenantId == tenantId &&
                    r.CreationTime.Date >= input.FromDate.Value.Date &&
                    r.CreationTime.Date <= input.ToDate.Value.Date &&
                    ips.ExpectedStartDate.HasValue && ips.Salary > 0

                    select new RequisitionInterviews
                    {
                        EnteredDate = r.StartDate,
                        SalesRep = s.FirstName + " " + s.MiddleName + " " + s.LastName,
                        Recruiter = rec.FirstName + " " + rec.MiddleName + " " + rec.LastName,
                        ConsultantName = cl.FirstName + " " + cl.MiddleName + " " + cl.LastName,
                        ConsultantId = cl.Id,
                        ClientName = c.ClientName,
                        ClientId = c.Id,
                        Requisition = r.JobTitle,
                        RequisitionId = r.Id,
                        StartDate = ips.ExpectedStartDate,
                        BillRate = a.BillRate ?? 0,
                        HourlyRate = a.PayRate ?? 0,

                    }).ToListAsync();

                var result = new MasterOfficeRptDto();
                result.RequisitionActivities = reqActivities;
                result.RequisitionInterviews = reqInterviews;

                return OutputHandler.Handler((int)ResponseType.GET, result, "Recruiter Report", 0);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<OutputDTO<List<AuditRptDto>>> AuditRptGet(AuditRptDto input)
        {
            try
            {
                var tenantId = _session.TenantId;

                var fromDate = input.FromDate?.Date ?? DateTime.Today;
                var toDate = input.ToDate?.Date.AddDays(1).AddTicks(-1) ?? DateTime.Today.AddDays(1).AddTicks(-1);

                // Start query
                var query = _context.UsersLogs.Where(x =>
                    x.TenantId == tenantId &&
                    x.CreationTime >= fromDate &&
                    x.CreationTime <= toDate
                );

                // Optional filtering by CreatorId
                if (!string.IsNullOrEmpty(input.CreatorId))
                {
                    query = query.Where(x => x.CreatorUserId == input.CreatorId);
                }

                var userlogs = await query
                    .OrderByDescending(o => o.Id)
                    .Select(s => new AuditRptDto
                    {
                        Message = s.Message,
                        Action = s.Action,
                        FormId = s.FormId,
                        FormName = s.FormName,
                        CreatorId = s.CreatorUserId,
                        CreationTime = s.CreationTime
                    })
                    .ToListAsync();

                foreach (var item in userlogs)
                {
                    var user = await _context.Users.Where(x => x.TenantId == tenantId && x.Id == item.CreatorId).FirstOrDefaultAsync();
                    item.CreatorName = $@"{user.FirstName} {user.LastName}";
                }

                return OutputHandler.Handler((int)ResponseType.GET, userlogs, "Audit Report", userlogs.Count);
            }
            catch (Exception ex)
            {
                throw;
            }

        }


    }
}
