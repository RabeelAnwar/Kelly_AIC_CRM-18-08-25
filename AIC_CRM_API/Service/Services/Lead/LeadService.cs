using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;
using LeadEntity = DataAccess.Entities.Lead;
using Microsoft.AspNetCore.Http;
using Service.Services.Lead;
using Service.Services.Lead.DTOs;
using Service.Sessions;
using Utility.OutputData;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Service.Services.Client.DTOs;
using Service.Services.UserLogs.DTOs;
using Service.Services.UserLogs;

namespace Service.Services.Leads
{
    public class LeadService : ILeadService
    {
        private readonly MainContext _context;
        private readonly IMapper _mapper;
        private readonly ISessionData _session;


        public LeadService(MainContext context, IMapper mapper, ISessionData sessionData)
        {
            _context = context;
            _mapper = mapper;
            _session = sessionData;
        }

        public async Task<OutputDTO<bool>> LeadAddUpdate(LeadInput input)
        {
            try
            {
                var existingLead = await _context.Leads
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == input.Id && x.TenantId == _session.TenantId);

                var mappedLead = _mapper.Map<LeadEntity>(input);

                LeadEntity leadEntitySaved;
                if (existingLead != null)
                {
                    var mapper = _mapper.Map(input, existingLead);
                    _context.Leads.Update(mapper);
                    await _context.SaveChangesAsync();
                    leadEntitySaved = mapper;
                }
                else
                {
                    _context.Leads.Add(mappedLead);
                    await _context.SaveChangesAsync();
                    leadEntitySaved = mappedLead;
                }

                var logInput = new UsersLogDto
                {
                    Action = existingLead != null ? "Update" : "Create",
                    FormName = "Lead",
                    FormId = leadEntitySaved.Id.ToString(),
                    Message = $"{(existingLead != null ? "Updated" : "Created")} Lead#{leadEntitySaved.Id}"
                };
                await _context.SaveUserLogAsync(logInput, _session, _mapper);

                return OutputHandler.Handler((int)(existingLead != null ? ResponseType.UPDATE : ResponseType.CREATE), true, "Lead");

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<OutputDTO<bool>> LeadDelete(int id)
        {
            try
            {
                var lead = await _context.Leads.FirstOrDefaultAsync(x => x.Id == id && x.TenantId == _session.TenantId);
                if (lead == null) return OutputHandler.Handler((int)ResponseType.NOT_EXIST, false, "Lead");

                _context.Leads.Remove(lead);
                await _context.SaveChangesAsync();

                return OutputHandler.Handler((int)ResponseType.DELETE, true, "Lead");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<OutputDTO<List<LeadInput>>> LeadsListGet()
        {
            try
            {
                var leadsWithDetails = await (
                    from lead in _context.Leads
                    join manager in _context.ClientManagers
                        on lead.ManagerId equals manager.Id into leadManagerGroup
                    from manager in leadManagerGroup.DefaultIfEmpty()

                    join assignedUser in _context.Users 
                        on lead.AssignedToId equals assignedUser.Id into assignedUserGroup
                    from assignedUser in assignedUserGroup.DefaultIfEmpty()

                    join generatedUser in _context.Users
                        on lead.CreatorUserId equals generatedUser.Id into generatedUserGroup
                    from generatedUser in generatedUserGroup.DefaultIfEmpty()

                    where lead.TenantId == _session.TenantId

                    select new LeadInput
                    {
                        Id = lead.Id,
                        ClientId = lead.ClientId,
                        Category = lead.Category,
                        LeadType = lead.LeadType,
                        StatusOfLead = lead.StatusOfLead,
                        DepartmentId = lead.DepartmentId,
                        ManagerId = lead.ManagerId,
                        ManagerName = manager != null ? manager.FirstName + " " + manager.LastName : null,
                        ManagerTitle = manager.Title,
                        AssignedToId = lead.AssignedToId,
                        AssignedTo = assignedUser != null ? assignedUser.FirstName + " " + assignedUser.LastName : null,
                        GeneratedBy = generatedUser != null ? generatedUser.FirstName + " " + generatedUser.LastName : null,
                        GeneratedById = lead.GeneratedById,
                        Called = lead.Called,
                        IsStaffConsultant = lead.IsStaffConsultant,
                        IsClientManager = lead.IsClientManager,
                        ReferredBy = lead.ReferredBy,
                        ApproximateAmount = lead.ApproximateAmount,
                        Source = lead.Source,
                        LeadInformation = lead.LeadInformation,
                        Result = lead.Result,
                        ReminderDateTime = lead.ReminderDateTime
                    }
                ).ToListAsync();

                return OutputHandler.Handler((int)ResponseType.GET, leadsWithDetails, "Leads", leadsWithDetails.Count);
            }
            catch (Exception)
            {
                throw;
            }
        }




        public async Task<OutputDTO<List<LeadListDto>>> DetailedLeadsListGet()
        {
            try
            {
                var leads = await (
                    from l in _context.Leads

                    join c in _context.Clients on l.ClientId equals c.Id into clientJoin
                    from client in clientJoin.DefaultIfEmpty()

                    join m in _context.ClientManagers on l.ManagerId equals m.Id into managerJoin
                    from manager in managerJoin.DefaultIfEmpty()

                    join assignedUser in _context.Users on l.AssignedToId equals assignedUser.Id into assignedUserJoin
                    from assignedToUser in assignedUserJoin.DefaultIfEmpty()

                    join generatedUser in _context.Users on l.CreatorUserId equals generatedUser.Id into generatedUserJoin
                    from generatedByUser in generatedUserJoin.DefaultIfEmpty()

                    join d in _context.Departments on l.DepartmentId equals d.Id into departmentJoin
                    from department in departmentJoin.DefaultIfEmpty()

                    where l.TenantId == _session.TenantId

                    select new LeadListDto
                    {
                        Id = l.Id,
                        ClientId = l.ClientId,
                        Category = l.Category,
                        LeadType = l.LeadType,
                        StatusOfLead = l.StatusOfLead,
                        DepartmentId = l.DepartmentId,
                        ManagerId = l.ManagerId,
                        AssignedToId = l.AssignedToId,
                        GeneratedById = l.GeneratedById,
                        Called = l.Called,
                        IsStaffConsultant = l.IsStaffConsultant,
                        IsClientManager = l.IsClientManager,
                        ReferredBy = l.ReferredBy,
                        ApproximateAmount = l.ApproximateAmount,
                        Source = l.Source,
                        LeadInformation = l.LeadInformation,
                        Result = l.Result,
                        ReminderDateTime = l.ReminderDateTime,

                        // Joined fields
                        ClientName = client.ClientName,
                        ManagerName = manager.FirstName + " " + (manager.MiddleName ?? "") + " " + manager.LastName,
                        ManagerTitle = manager.Title,
                        ManagerContact = manager.CellPhone,
                        ManagerEmail = manager.WorkEmail,

                        AssignedTo = assignedToUser.FirstName + " " + assignedToUser.LastName,
                        GeneratedBy = generatedByUser.FirstName + " " + generatedByUser.LastName,
                        DepartmentName = department.Name
                    }
                ).ToListAsync();

                return OutputHandler.Handler((int)ResponseType.GET, leads, "Leads", leads.Count);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<OutputDTO<LeadInput>> SingleLeadGet()
        {
            try
            {
                var lead = await _context.Leads
                    .Where(x => x.TenantId == _session.TenantId)
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefaultAsync();

                var mappedLead = _mapper.Map<LeadInput>(lead);
                return OutputHandler.Handler((int)ResponseType.GET, mappedLead, "Lead", 0);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }

}
