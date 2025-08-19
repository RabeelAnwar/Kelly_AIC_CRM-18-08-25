using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Service.Services.Consultant.DTOs;
using Service.Services.Requisition.DTOs;
using Service.Sessions;
using Utility.OutputData;

namespace Service.Services.Requisition
{
    public class RequisitionService : IRequisitionService
    {
        private readonly MainContext _context;
        private readonly IMapper _mapper;
        private readonly ISessionData _session;

        public RequisitionService(MainContext context, IMapper mapper, ISessionData sessionData)
        {
            _context = context;
            _mapper = mapper;
            _session = sessionData;
        }

        public async Task<OutputDTO<RequisitionDto>> ClientRequisitionAddUpdate(RequisitionDto input)
        {
            try
            {
                var existing = await _context.ClientRequisitions
                    //.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == input.Id && x.TenantId == _session.TenantId);

                var mapped = _mapper.Map<ClientRequisition>(input);
                mapped.TenantId = _session.TenantId;

                ClientRequisition saved;

                if (existing != null)
                {
                    var updatedEntity = _mapper.Map(input, existing);
                    updatedEntity.Id = existing.Id;
                    updatedEntity.TenantId = _session.TenantId;

                    _context.ClientRequisitions.Update(updatedEntity);
                    saved = updatedEntity;
                }
                else
                {
                    mapped.Status = true;
                    _context.ClientRequisitions.Add(mapped);
                    saved = mapped;
                }

                await _context.SaveChangesAsync();

                var resultDto = _mapper.Map<RequisitionDto>(saved);
                return OutputHandler.Handler((int)(existing != null ? ResponseType.UPDATE : ResponseType.CREATE), resultDto, "Client Requisition");
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<OutputDTO<bool>> ClientRequisitionStatusUpdate(RequisitionDto input)
        {
            try
            {
                var existing = await _context.ClientRequisitions.Where(x => x.Id == input.Id).FirstOrDefaultAsync();
                existing.Status = input.Status;
                _context.ClientRequisitions.Update(existing);
                await _context.SaveChangesAsync();

                return OutputHandler.Handler((int)(existing != null ? ResponseType.UPDATE : ResponseType.CREATE), true, "Requisition Status");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<OutputDTO<bool>> ClientRequisitionDelete(int id)
        {
            try
            {
                var requisition = await _context.ClientRequisitions
                    .FirstOrDefaultAsync(x => x.Id == id && x.TenantId == _session.TenantId);

                if (requisition == null)
                    return OutputHandler.Handler((int)ResponseType.NOT_EXIST, false, "Client Requisition");

                _context.ClientRequisitions.Remove(requisition);
                await _context.SaveChangesAsync();
                return OutputHandler.Handler((int)ResponseType.DELETE, true, "Client Requisition");
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<OutputDTO<List<RequisitionDto>>> ClientRequisitionsListGet()
        {
            try
            {
                var requisitions = await (from req in _context.ClientRequisitions
                                          where req.TenantId == _session.TenantId
                                          join client in _context.Clients on req.ClientId equals client.Id into clientJoin
                                          from client in clientJoin.DefaultIfEmpty()
                                          join manager in _context.ClientManagers on req.ManagerId equals manager.Id into managerJoin
                                          from manager in managerJoin.DefaultIfEmpty()
                                          orderby req.Id descending
                                          select new RequisitionDto
                                          {
                                              Id = req.Id,
                                              ClientId = req.ClientId,
                                              ClientName = client != null ? client.ClientName : null,
                                              ManagerId = req.ManagerId,
                                              ManagerName = manager != null ? manager.FirstName + " " + manager.LastName : null,
                                              InternalReqCoordinatorId = req.InternalReqCoordinatorId,
                                              RequisitionType = req.RequisitionType,
                                              Priority = req.Priority,
                                              JobTitle = req.JobTitle,
                                              ClientReqNumber = req.ClientReqNumber,
                                              SalesRepId = req.SalesRepId,
                                              RecruiterAssignedId = req.RecruiterAssignedId,
                                              Location = req.Location,
                                              Duration = req.Duration,
                                              DurationTypes = req.DurationTypes,
                                              StartDate = req.StartDate,
                                              NumberOfPositions = req.NumberOfPositions,
                                              Comments = req.Comments,
                                              ProjectDepartmentOverview = req.ProjectDepartmentOverview,
                                              JobDescription = req.JobDescription,
                                              PayRate = req.PayRate,
                                              BillRate = req.BillRate,
                                              Hours = req.Hours,
                                              Overtime = req.Overtime,
                                              InterviewProcesses = req.InterviewProcesses,
                                              PhoneHireIfOutOfArea = req.PhoneHireIfOutOfArea,
                                              ClientMarkup = req.ClientMarkup,
                                              BillRateHighestBeforeResumeNotSent = req.BillRateHighestBeforeResumeNotSent,
                                              SecondaryContact = req.SecondaryContact,
                                              HiringManagerVop = req.HiringManagerVop,
                                              OtherWaysToFillPosition = req.OtherWaysToFillPosition,
                                              Notes = req.Notes,
                                              Responsibilities = req.Responsibilities,
                                              Qualifications = req.Qualifications,
                                              SearchString1 = req.SearchString1,
                                              SearchString2 = req.SearchString2,
                                              SearchString3 = req.SearchString3,
                                              CodingValue = req.CodingValue,
                                              AnalysisValue = req.AnalysisValue,
                                              TestingValue = req.TestingValue,
                                              OtherValue = req.OtherValue,
                                              Hardware = req.Hardware,
                                              OS = req.OS,
                                              Languages = req.Languages,
                                              Databases = req.Databases,
                                              Protocols = req.Protocols,
                                              SoftwareStandards = req.SoftwareStandards,
                                              Others = req.Others,
                                              Status = req.Status,
                                              CreatedDate = req.CreationTime,
                                              InternalReqCoordinatorName = _context.Users
                                                .Where(x => x.Id == req.InternalReqCoordinatorId)
                                                .Select(s => s.FirstName + s.LastName)
                                                .FirstOrDefault(),
                                              RecruiterAssignedName = _context.Users
                                                .Where(x => req.RecruiterAssignedId.Contains(x.Id))
                                                .Select(s => s.FirstName + s.LastName)
                                                .ToList(),
                                              SalesRepName = _context.Users
                                                .Where(x => x.Id == req.SalesRepId)
                                                .Select(s => s.FirstName + s.LastName)
                                                .FirstOrDefault()
                                          }).ToListAsync();

                return OutputHandler.Handler((int)ResponseType.GET, requisitions, "Client Requisitions", requisitions.Count);
            }
            catch (Exception)
            {
                throw;
            }
        }



        public async Task<OutputDTO<List<RequisitionDto>>> ClientOpenRequisitionsListGet()
        {
            try
            {
                var requisitions = await (from req in _context.ClientRequisitions
                                          where req.TenantId == _session.TenantId && req.Status == true
                                          join client in _context.Clients on req.ClientId equals client.Id into clientJoin
                                          from client in clientJoin.DefaultIfEmpty()
                                          join manager in _context.ClientManagers on req.ManagerId equals manager.Id into managerJoin
                                          from manager in managerJoin.DefaultIfEmpty()
                                          orderby req.Id descending
                                          select new RequisitionDto
                                          {
                                              Id = req.Id,
                                              ClientId = req.ClientId,
                                              ClientName = client != null ? client.ClientName : null,
                                              ManagerId = req.ManagerId,
                                              ManagerName = manager != null ? manager.FirstName + " " + manager.LastName : null,
                                              InternalReqCoordinatorId = req.InternalReqCoordinatorId,
                                              RequisitionType = req.RequisitionType,
                                              Priority = req.Priority,
                                              JobTitle = req.JobTitle,
                                              ClientReqNumber = req.ClientReqNumber,
                                              SalesRepId = req.SalesRepId,
                                              RecruiterAssignedId = req.RecruiterAssignedId,
                                              Location = req.Location,
                                              Duration = req.Duration,
                                              DurationTypes = req.DurationTypes,
                                              StartDate = req.StartDate,
                                              NumberOfPositions = req.NumberOfPositions,
                                              Comments = req.Comments,
                                              ProjectDepartmentOverview = req.ProjectDepartmentOverview,
                                              JobDescription = req.JobDescription,
                                              PayRate = req.PayRate,
                                              BillRate = req.BillRate,
                                              Hours = req.Hours,
                                              Overtime = req.Overtime,
                                              InterviewProcesses = req.InterviewProcesses,
                                              PhoneHireIfOutOfArea = req.PhoneHireIfOutOfArea,
                                              ClientMarkup = req.ClientMarkup,
                                              BillRateHighestBeforeResumeNotSent = req.BillRateHighestBeforeResumeNotSent,
                                              SecondaryContact = req.SecondaryContact,
                                              HiringManagerVop = req.HiringManagerVop,
                                              OtherWaysToFillPosition = req.OtherWaysToFillPosition,
                                              Notes = req.Notes,
                                              Responsibilities = req.Responsibilities,
                                              Qualifications = req.Qualifications,
                                              SearchString1 = req.SearchString1,
                                              SearchString2 = req.SearchString2,
                                              SearchString3 = req.SearchString3,
                                              CodingValue = req.CodingValue,
                                              AnalysisValue = req.AnalysisValue,
                                              TestingValue = req.TestingValue,
                                              OtherValue = req.OtherValue,
                                              Hardware = req.Hardware,
                                              OS = req.OS,
                                              Languages = req.Languages,
                                              Databases = req.Databases,
                                              Protocols = req.Protocols,
                                              SoftwareStandards = req.SoftwareStandards,
                                              Others = req.Others,
                                              Status = req.Status,
                                              CreatedDate = req.CreationTime,
                                              InternalReqCoordinatorName = _context.Users
                                                .Where(x => x.Id == req.InternalReqCoordinatorId)
                                                .Select(s => s.FirstName + s.LastName)
                                                .FirstOrDefault(),
                                              RecruiterAssignedName = _context.Users
                                                .Where(x => req.RecruiterAssignedId.Contains(x.Id))
                                                .Select(s => s.FirstName + s.LastName)
                                                .ToList(),
                                              SalesRepName = _context.Users
                                                .Where(x => x.Id == req.SalesRepId)
                                                .Select(s => s.FirstName + s.LastName)
                                                .FirstOrDefault()
                                          }).ToListAsync();

                return OutputHandler.Handler((int)ResponseType.GET, requisitions, "Client Requisitions", requisitions.Count);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<OutputDTO<List<RequisitionDto>>> ClientRequisitionsListGetByClientId(int clientId)
        {
            try
            {
                var requisitions = await (from req in _context.ClientRequisitions
                                          where req.TenantId == _session.TenantId && req.ClientId == clientId
                                          join client in _context.Clients on req.ClientId equals client.Id into clientJoin
                                          from client in clientJoin.DefaultIfEmpty()
                                          join manager in _context.ClientManagers on req.ManagerId equals manager.Id into managerJoin
                                          from manager in managerJoin.DefaultIfEmpty()
                                          orderby req.Id descending
                                          select new RequisitionDto
                                          {
                                              Id = req.Id,
                                              ClientId = req.ClientId,
                                              ClientName = client != null ? client.ClientName : null,
                                              ManagerId = req.ManagerId,
                                              ManagerName = manager != null ? manager.FirstName + " " + manager.LastName : null,
                                              InternalReqCoordinatorId = req.InternalReqCoordinatorId,
                                              RequisitionType = req.RequisitionType,
                                              Priority = req.Priority,
                                              JobTitle = req.JobTitle,
                                              ClientReqNumber = req.ClientReqNumber,
                                              SalesRepId = req.SalesRepId,
                                              RecruiterAssignedId = req.RecruiterAssignedId,
                                              Location = req.Location,
                                              Duration = req.Duration,
                                              DurationTypes = req.DurationTypes,
                                              StartDate = req.StartDate,
                                              NumberOfPositions = req.NumberOfPositions,
                                              Comments = req.Comments,
                                              ProjectDepartmentOverview = req.ProjectDepartmentOverview,
                                              JobDescription = req.JobDescription,
                                              PayRate = req.PayRate,
                                              BillRate = req.BillRate,
                                              Hours = req.Hours,
                                              Overtime = req.Overtime,
                                              InterviewProcesses = req.InterviewProcesses,
                                              PhoneHireIfOutOfArea = req.PhoneHireIfOutOfArea,
                                              ClientMarkup = req.ClientMarkup,
                                              BillRateHighestBeforeResumeNotSent = req.BillRateHighestBeforeResumeNotSent,
                                              SecondaryContact = req.SecondaryContact,
                                              HiringManagerVop = req.HiringManagerVop,
                                              OtherWaysToFillPosition = req.OtherWaysToFillPosition,
                                              Notes = req.Notes,
                                              Responsibilities = req.Responsibilities,
                                              Qualifications = req.Qualifications,
                                              SearchString1 = req.SearchString1,
                                              SearchString2 = req.SearchString2,
                                              SearchString3 = req.SearchString3,
                                              CodingValue = req.CodingValue,
                                              AnalysisValue = req.AnalysisValue,
                                              TestingValue = req.TestingValue,
                                              OtherValue = req.OtherValue,
                                              Hardware = req.Hardware,
                                              OS = req.OS,
                                              Languages = req.Languages,
                                              Databases = req.Databases,
                                              Protocols = req.Protocols,
                                              SoftwareStandards = req.SoftwareStandards,
                                              Others = req.Others,
                                              Status = req.Status,
                                              CreatedDate = req.CreationTime,
                                              InternalReqCoordinatorName = _context.Users
                                                .Where(x => x.Id == req.InternalReqCoordinatorId)
                                                .Select(s => s.FirstName + s.LastName)
                                                .FirstOrDefault(),
                                              RecruiterAssignedName = _context.Users
                                                .Where(x => req.RecruiterAssignedId.Contains(x.Id))
                                                .Select(s => s.FirstName + s.LastName)
                                                .ToList(),
                                              SalesRepName = _context.Users
                                                .Where(x => x.Id == req.SalesRepId)
                                                .Select(s => s.FirstName + s.LastName)
                                                .FirstOrDefault()
                                          }).ToListAsync();

                return OutputHandler.Handler((int)ResponseType.GET, requisitions, "Client Requisitions", requisitions.Count);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<OutputDTO<RequisitionDto>> ClientRequisitionGet(int id)
        {
            try
            {
                var requisition = await (from req in _context.ClientRequisitions
                                         where req.Id == id && req.TenantId == _session.TenantId
                                         join client in _context.Clients on req.ClientId equals client.Id into clientJoin
                                         from client in clientJoin.DefaultIfEmpty()
                                         join manager in _context.ClientManagers on req.ManagerId equals manager.Id into managerJoin
                                         from manager in managerJoin.DefaultIfEmpty()
                                         select new RequisitionDto
                                         {
                                             Id = req.Id,
                                             ClientId = req.ClientId,
                                             ClientName = client != null ? client.ClientName : null,
                                             ManagerId = req.ManagerId,
                                             ManagerName = manager != null ? manager.FirstName + " " + manager.LastName : null,
                                             InternalReqCoordinatorId = req.InternalReqCoordinatorId,
                                             RequisitionType = req.RequisitionType,
                                             Priority = req.Priority,
                                             JobTitle = req.JobTitle,
                                             ClientReqNumber = req.ClientReqNumber,
                                             SalesRepId = req.SalesRepId,
                                             RecruiterAssignedId = req.RecruiterAssignedId,
                                             Location = req.Location,
                                             Duration = req.Duration,
                                             DurationTypes = req.DurationTypes,
                                             StartDate = req.StartDate,
                                             CreatedDate = req.CreationTime,
                                             NumberOfPositions = req.NumberOfPositions,
                                             Comments = req.Comments,
                                             ProjectDepartmentOverview = req.ProjectDepartmentOverview,
                                             JobDescription = req.JobDescription,
                                             PayRate = req.PayRate,
                                             BillRate = req.BillRate,
                                             Hours = req.Hours,
                                             Overtime = req.Overtime,
                                             InterviewProcesses = req.InterviewProcesses,
                                             PhoneHireIfOutOfArea = req.PhoneHireIfOutOfArea,
                                             ClientMarkup = req.ClientMarkup,
                                             BillRateHighestBeforeResumeNotSent = req.BillRateHighestBeforeResumeNotSent,
                                             SecondaryContact = req.SecondaryContact,
                                             HiringManagerVop = req.HiringManagerVop,
                                             OtherWaysToFillPosition = req.OtherWaysToFillPosition,
                                             Notes = req.Notes,
                                             Responsibilities = req.Responsibilities,
                                             Qualifications = req.Qualifications,
                                             SearchString1 = req.SearchString1,
                                             SearchString2 = req.SearchString2,
                                             SearchString3 = req.SearchString3,
                                             CodingValue = req.CodingValue,
                                             AnalysisValue = req.AnalysisValue,
                                             TestingValue = req.TestingValue,
                                             OtherValue = req.OtherValue,
                                             Hardware = req.Hardware,
                                             OS = req.OS,
                                             Languages = req.Languages,
                                             Databases = req.Databases,
                                             Protocols = req.Protocols,
                                             SoftwareStandards = req.SoftwareStandards,
                                             Others = req.Others,
                                             Status = req.Status,
                                             InternalReqCoordinatorName = _context.Users
                                                 .Where(x => x.Id == req.InternalReqCoordinatorId)
                                                 .Select(s => s.FirstName + s.LastName)
                                                 .FirstOrDefault(),
                                             RecruiterAssignedName = _context.Users
                                                 .Where(x => req.RecruiterAssignedId.Contains(x.Id))
                                                 .Select(s => s.FirstName + s.LastName)
                                                 .ToList(),
                                             SalesRepName = _context.Users
                                                 .Where(x => x.Id == req.SalesRepId)
                                                 .Select(s => s.FirstName + s.LastName)
                                                 .FirstOrDefault()
                                         }).FirstOrDefaultAsync();

                if (requisition == null)
                    return OutputHandler.Handler<RequisitionDto>((int)ResponseType.NOT_EXIST, null, "Client Requisition");

                return OutputHandler.Handler((int)ResponseType.GET, requisition, "Client Requisition");
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<OutputDTO<List<RequisitionDto>>> ClientRequisitionGetByClientId(int clientId)
        {
            try
            {
                var requisitions = await (from req in _context.ClientRequisitions
                                          where req.ClientId == clientId && req.TenantId == _session.TenantId
                                          join client in _context.Clients on req.ClientId equals client.Id into clientJoin
                                          from client in clientJoin.DefaultIfEmpty()
                                          join manager in _context.ClientManagers on req.ManagerId equals manager.Id into managerJoin
                                          from manager in managerJoin.DefaultIfEmpty()
                                          orderby req.Id descending
                                          select new RequisitionDto
                                          {
                                              Id = req.Id,
                                              ClientId = req.ClientId,
                                              ClientName = client != null ? client.ClientName : null,
                                              ManagerId = req.ManagerId,
                                              ManagerName = manager != null ? manager.FirstName + " " + manager.LastName : null,
                                              InternalReqCoordinatorId = req.InternalReqCoordinatorId,
                                              RequisitionType = req.RequisitionType,
                                              Priority = req.Priority,
                                              JobTitle = req.JobTitle,
                                              ClientReqNumber = req.ClientReqNumber,
                                              SalesRepId = req.SalesRepId,
                                              RecruiterAssignedId = req.RecruiterAssignedId,
                                              Location = req.Location,
                                              Duration = req.Duration,
                                              DurationTypes = req.DurationTypes,
                                              StartDate = req.StartDate,
                                              NumberOfPositions = req.NumberOfPositions,
                                              Comments = req.Comments,
                                              ProjectDepartmentOverview = req.ProjectDepartmentOverview,
                                              JobDescription = req.JobDescription,
                                              PayRate = req.PayRate,
                                              BillRate = req.BillRate,
                                              Hours = req.Hours,
                                              Overtime = req.Overtime,
                                              InterviewProcesses = req.InterviewProcesses,
                                              PhoneHireIfOutOfArea = req.PhoneHireIfOutOfArea,
                                              ClientMarkup = req.ClientMarkup,
                                              BillRateHighestBeforeResumeNotSent = req.BillRateHighestBeforeResumeNotSent,
                                              SecondaryContact = req.SecondaryContact,
                                              HiringManagerVop = req.HiringManagerVop,
                                              OtherWaysToFillPosition = req.OtherWaysToFillPosition,
                                              Notes = req.Notes,
                                              Responsibilities = req.Responsibilities,
                                              Qualifications = req.Qualifications,
                                              SearchString1 = req.SearchString1,
                                              SearchString2 = req.SearchString2,
                                              SearchString3 = req.SearchString3,
                                              CodingValue = req.CodingValue,
                                              AnalysisValue = req.AnalysisValue,
                                              TestingValue = req.TestingValue,
                                              OtherValue = req.OtherValue,
                                              Hardware = req.Hardware,
                                              OS = req.OS,
                                              Languages = req.Languages,
                                              Databases = req.Databases,
                                              Protocols = req.Protocols,
                                              SoftwareStandards = req.SoftwareStandards,
                                              Others = req.Others,
                                              InternalReqCoordinatorName = _context.Users
                                                  .Where(x => x.Id == req.InternalReqCoordinatorId)
                                                  .Select(s => s.FirstName + s.LastName)
                                                  .FirstOrDefault(),
                                              RecruiterAssignedName = _context.Users
                                                  .Where(x => req.RecruiterAssignedId.Contains(x.Id))
                                                  .Select(s => s.FirstName + s.LastName)
                                                  .ToList(),
                                              SalesRepName = _context.Users
                                                  .Where(x => x.Id == req.SalesRepId)
                                                  .Select(s => s.FirstName + s.LastName)
                                                  .FirstOrDefault()
                                          }).ToListAsync();

                return OutputHandler.Handler((int)ResponseType.GET, requisitions, "Client Requisitions", requisitions.Count);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }

}