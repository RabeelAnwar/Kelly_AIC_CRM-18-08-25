using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Service.Sessions;
using Utility.OutputData;
using Microsoft.EntityFrameworkCore;
using CallRecordEntity = DataAccess.Entities.CallRecord;
using Service.Services.CallRecord.DTOs;
using AutoMapper;
using Service.Services.UserLogs.DTOs;
using Service.Services.UserLogs;
using System.Reflection.Metadata;


namespace Service.Services.CallRecord
{
    public class CallRecordService : ICallRecordService
    {


        private readonly MainContext _context;
        private readonly IMapper _mapper;
        private readonly ISessionData _session;


        public CallRecordService(MainContext context, IMapper mapper, ISessionData sessionData)
        {
            _context = context;
            _mapper = mapper;
            _session = sessionData;
        }

        public async Task<OutputDTO<bool>> AddOrUpdateCallRecord(CallRecordDto input)
        {
            try
            {
                var existing = await _context.CallRecords
                    .FirstOrDefaultAsync(x => x.Id == input.Id && x.TenantId == _session.TenantId);

                var mapped = _mapper.Map<CallRecordEntity>(input);
                CallRecordEntity callRecordSaved;


                if (existing != null)
                {
                    var mapper = _mapper.Map(input, existing);
                    _context.CallRecords.Update(mapper);
                    await _context.SaveChangesAsync();
                    callRecordSaved = mapper;
                }
                else
                {
                    _context.CallRecords.Add(mapped);
                    await _context.SaveChangesAsync();
                    callRecordSaved = mapped;
                }

                var logInput = new UsersLogDto
                {
                    Action = existing != null ? "Update" : "Create",
                    FormName = "Call Record",
                    FormId = callRecordSaved.Id.ToString(),
                    Message = $"{(existing != null ? "Updated" : "Created")} call record for {(callRecordSaved.ClientId > 0 ?
                    "Client " +  _context.Clients.Where( x => x.Id == callRecordSaved.ClientId).Select(s => s.ClientName).FirstOrDefault():
                    (callRecordSaved.ConsultantId > 0 ?
                    "Consultant " + _context.Consultants.Where(x => x.Id == callRecordSaved.ConsultantId).Select(s => (s.FirstName + " " + s.LastName )).FirstOrDefault() :
                    (callRecordSaved.ManagerId > 0 ?
                    "Manager " + _context.ClientManagers.Where(x => x.Id == callRecordSaved.ManagerId).Select(s => (s.FirstName + " " + s.LastName)).FirstOrDefault() :
                    callRecordSaved.Id)))}"
                };

                await _context.SaveUserLogAsync(logInput, _session, _mapper);

                return OutputHandler.Handler((int)(existing != null ? ResponseType.UPDATE : ResponseType.CREATE), true, "Call Record");

            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<OutputDTO<bool>> DeleteCallRecord(int id)
        {
            try
            {
                var record = await _context.CallRecords
                    .FirstOrDefaultAsync(x => x.Id == id && x.TenantId == _session.TenantId);

                if (record == null) return OutputHandler.Handler((int)ResponseType.NOT_EXIST, false, "Call Record");

                _context.CallRecords.Remove(record);
                await _context.SaveChangesAsync();

                var logInput = new UsersLogDto
                {
                    Action = "Delete",
                    FormName = "Call Record",
                    FormId = id.ToString(),
                    Message = $"Delete call record for {(record.ClientId > 0 ?
                    "Client " + _context.Clients.Where(x => x.Id == record.ClientId).Select(s => s.ClientName).FirstOrDefault() :
                    (record.ConsultantId > 0 ?
                    "Consultant " + _context.Consultants.Where(x => x.Id == record.ConsultantId).Select(s => (s.FirstName + " " + s.LastName)).FirstOrDefault() :
                    (record.ManagerId > 0 ?
                    "Manager " + _context.ClientManagers.Where(x => x.Id == record.ManagerId).Select(s => (s.FirstName + " " + s.LastName)).FirstOrDefault() :
                    record.Id)))}"
                };
                await _context.SaveUserLogAsync(logInput, _session, _mapper);

                return OutputHandler.Handler((int)ResponseType.DELETE, true, "Call Record");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<OutputDTO<List<CallRecordDto>>> CallRecordsListGet(int leadsId, int managerId, int consultantId)
        {
            try
            {
                var query = from call in _context.CallRecords
                            join user in _context.Users
                                on call.CreatorUserId equals user.Id into userGroup
                            from creator in userGroup.DefaultIfEmpty()
                            where call.TenantId == _session.TenantId
                            select new CallRecordDto
                            {
                                Id = call.Id,
                                ClientId = call.ClientId,
                                LeadId = call.LeadId,
                                ManagerId = call.ManagerId,
                                ConsultantId = call.ConsultantId,
                                Date = call.Date,
                                TypeId = call.TypeId,
                                Record = call.Record,
                                RemindStatus = call.RemindStatus,
                                ReminderDate = call.ReminderDate,
                                CreatorUserName = creator != null ? creator.LastName + creator.FirstName : null
                            };

                if (leadsId > 0)
                    query = query.Where(x => x.LeadId == leadsId);

                if (managerId > 0)
                    query = query.Where(x => x.ManagerId == managerId);

                if (consultantId > 0)
                    query = query.Where(x => x.ConsultantId == consultantId);

                var result = await query.OrderByDescending(o => o.Id).ToListAsync();

                return OutputHandler.Handler((int)ResponseType.GET, result, "Call Records", result.Count);
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }




    }
}
