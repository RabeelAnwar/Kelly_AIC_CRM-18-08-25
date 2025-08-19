using DataAccess.Entities;
using ClientEntity = DataAccess.Entities.Client;
using Microsoft.EntityFrameworkCore;
using Service.Services.Client.DTOs;
using Service.Sessions;
using Utility.OutputData;
using Service.Services.UserLogs.DTOs;
using Service.Services.UserLogs;

namespace Service.Services.Client
{
    public class ClientService : IClientService
    {
        private readonly MainContext _context;
        private readonly IMapper _mapper;
        private readonly ISessionData _session;

        public ClientService(MainContext context, IMapper mapper, ISessionData sessionData)
        {
            _context = context;
            _mapper = mapper;
            _session = sessionData;
        }

        #region Client


        public async Task<OutputDTO<ClientInput>> ClientAddUpdate(ClientInput input)
        {
            try
            {
                var existingClient = await _context.Clients
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == input.Id && x.TenantId == _session.TenantId);

                var mappedClient = _mapper.Map<ClientEntity>(input);
                mappedClient.TenantId = _session.TenantId;

                ClientEntity savedClient;

                if (existingClient != null)
                {
                    _mapper.Map(input, existingClient); // maps values into existing entity
                    existingClient.TenantId = _session.TenantId;
                    _context.Clients.Update(existingClient);
                    await _context.SaveChangesAsync();
                    savedClient = existingClient;
                }
                else
                {
                    _context.Clients.Add(mappedClient);
                    await _context.SaveChangesAsync();
                    savedClient = mappedClient;
                }

                var logInput = new UsersLogDto
                {
                    Action = existingClient != null ? "Update" : "Create",
                    FormName = "Client",
                    FormId = savedClient.Id.ToString(),
                    Message = $"{(existingClient != null ? "Updated" : "Created")} Client {savedClient.ClientName}"
                };
                await _context.SaveUserLogAsync(logInput, _session, _mapper);

                var resultDto = _mapper.Map<ClientInput>(savedClient);
                return OutputHandler.Handler((int)(existingClient != null ? ResponseType.UPDATE : ResponseType.CREATE), resultDto, "Client");
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<OutputDTO<bool>> ClientDelete(int id)
        {
            try
            {
                var client = await _context.Clients.FirstOrDefaultAsync(x => x.Id == id && x.TenantId == _session.TenantId);
                if (client == null) return OutputHandler.Handler((int)ResponseType.NOT_EXIST, false, "Client");

                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();

                var logInput = new UsersLogDto
                {
                    Action = "Delete",
                    FormName = "Client",
                    FormId = id.ToString(),
                    Message = $"Delete Client {client.ClientName}"
                };
                await _context.SaveUserLogAsync(logInput, _session, _mapper);

                return OutputHandler.Handler((int)ResponseType.DELETE, true, "Client");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<OutputDTO<List<ClientInput>>> ClientsListGet()
        {
            try
            {
                var clients = await _context.Clients
                    .Where(x => x.TenantId == _session.TenantId).OrderBy(i => i.ClientName)
                    .ToListAsync();

                var mappedClients = _mapper.Map<List<ClientInput>>(clients);
                return OutputHandler.Handler((int)ResponseType.GET, mappedClients, "Clients", mappedClients.Count);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<OutputDTO<ClientInput>> SingleClientGet(int id)
        {
            try
            {
                var client = await _context.Clients
                    .Where(x => x.TenantId == _session.TenantId && x.Id == id)
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefaultAsync();

                var mappedClient = _mapper.Map<ClientInput>(client);
                return OutputHandler.Handler((int)ResponseType.GET, mappedClient, "Client", 0);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion



        #region Client Manager



        public async Task<OutputDTO<ClientManagerDto>> ClientManagerAddUpdate(ClientManagerDto input)
        {
            try
            {
                var existing = await _context.ClientManagers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == input.Id && x.TenantId == _session.TenantId);

                var mapped = _mapper.Map<ClientManager>(input);
                mapped.TenantId = _session.TenantId;

                ClientManager saved;

                if (existing != null)
                {
                    var updatedEntity = _mapper.Map(input, existing);
                    updatedEntity.Id = existing.Id;
                    updatedEntity.TenantId = _session.TenantId;

                    _context.ClientManagers.Update(updatedEntity);
                    saved = updatedEntity;
                }
                else
                {
                    _context.ClientManagers.Add(mapped);
                    saved = mapped;
                }

                await _context.SaveChangesAsync();


                var logInput = new UsersLogDto
                {
                    Action = existing != null ? "Update" : "Create",
                    FormName = "Manager",
                    FormId = saved.Id.ToString(),
                    Message = $"{(existing != null ? "Updated" : "Created")} Manager {saved.FirstName} {saved.LastName}"
                };
                await _context.SaveUserLogAsync(logInput, _session, _mapper);

                var resultDto = _mapper.Map<ClientManagerDto>(saved);
                return OutputHandler.Handler((int)(existing != null ? ResponseType.UPDATE : ResponseType.CREATE), resultDto, "Client Manager");
            }
            catch (Exception)
            {
                throw; // Consider logging if you want better diagnostics
            }
        }


        public async Task<OutputDTO<bool>> ClientManagerDelete(int id)
        {
            try
            {
                var manager = await _context.ClientManagers
                    .FirstOrDefaultAsync(x => x.Id == id && x.TenantId == _session.TenantId);

                if (manager == null)
                    return OutputHandler.Handler((int)ResponseType.NOT_EXIST, false, "Client Manager");

                _context.ClientManagers.Remove(manager);
                await _context.SaveChangesAsync();

                var logInput = new UsersLogDto
                {
                    Action = "Delete",
                    FormName = "Manager",
                    FormId = id.ToString(),
                    Message = $"Delete Manager {manager.FirstName} {manager.LastName}"
                };
                await _context.SaveUserLogAsync(logInput, _session, _mapper);
                return OutputHandler.Handler((int)ResponseType.DELETE, true, "Client Manager");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<OutputDTO<List<ClientManagerDto>>> ClientManagersListGet()
        {
            try
            {
                var managers = await (
                    from cm in _context.ClientManagers
                    join c in _context.Clients
                        on cm.ClientId equals c.Id
                    where cm.TenantId == _session.TenantId
                    orderby cm.Id
                    select new ClientManagerDto
                    {
                        Id = cm.Id,
                        ClientId = cm.ClientId,
                        ClientName = c.ClientName,
                        FirstName = cm.FirstName,
                        MiddleName = cm.MiddleName,
                        LastName = cm.LastName,
                        Title = cm.Title,
                        DepartmentId = cm.DepartmentId,
                        WorksUnderId = cm.WorksUnderId,
                        IsAssignedToId = cm.IsAssignedToId,
                        Address1 = cm.Address1,
                        Address2 = cm.Address2,
                        Country = cm.Country,
                        State = cm.State,
                        City = cm.City,
                        ZipCode = cm.ZipCode,
                        OfficePhone = cm.OfficePhone,
                        OfficePhoneExt = cm.OfficePhoneExt,
                        CellPhone = cm.CellPhone,
                        CellPhoneExt = cm.CellPhoneExt,
                        HomePhone = cm.HomePhone,
                        HomePhoneExt = cm.HomePhoneExt,
                        WorkEmail = cm.WorkEmail,
                        PersonalEmail = cm.PersonalEmail,
                        LinkedInUrl = cm.LinkedInUrl,
                        LinkedInImageUrl = cm.LinkedInImageUrl,
                        SkillNeeded = cm.SkillNeeded,
                        Notes = cm.Notes,
                        IsManager = cm.IsManager,
                        StillWithCompany = cm.StillWithCompany,
                        IsActive = cm.IsActive,
                        CallRecords = _context.CallRecords
                                                        .Where(cr => cr.ManagerId == cm.Id)
                                                        .OrderByDescending(cr => cr.Id)
                                                        .Select(cr => cr.Record)
                                                        .FirstOrDefault()
                    }).OrderBy(x => x.LastName).ToListAsync();

                managers.OrderBy(o => o.LastName).ToList();

                return OutputHandler.Handler((int)ResponseType.GET, managers, "Client Managers", managers.Count);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<OutputDTO<ClientManagerDto>> ClientManagerGet(int id)
        {
            try
            {
                var manager = await _context.ClientManagers
                    .FirstOrDefaultAsync(x => x.Id == id && x.TenantId == _session.TenantId);


                if (manager == null)
                    return OutputHandler.Handler<ClientManagerDto>((int)ResponseType.NOT_EXIST, null, "Client Manager");


                var mapped = _mapper.Map<ClientManagerDto>(manager);
                return OutputHandler.Handler((int)ResponseType.GET, mapped, "Client Manager");
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<OutputDTO<List<ClientManagerDto>>> ClientManagerGetByClientId(int id)
        {
            try
            {

                var manager = await _context.ClientManagers.Where(x => x.ClientId == id && x.TenantId == _session.TenantId).OrderBy(o => o.LastName).ToListAsync();
                var managers = _mapper.Map<List<ClientManagerDto>>(manager);
                return OutputHandler.Handler((int)ResponseType.GET, managers, "Client Managers", managers.Count);

            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<OutputDTO<List<ClientManagerDto>>> GetWorkUnderManagers(int id)
        {
            try
            {
                var manager = await _context.ClientManagers.Where(x => x.ClientId != id && x.TenantId == _session.TenantId).ToListAsync();
                var managers = _mapper.Map<List<ClientManagerDto>>(manager);
                return OutputHandler.Handler((int)ResponseType.GET, managers, "Client Managers", managers.Count);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion



        #region Client Pipeline
        public async Task<OutputDTO<bool>> ClientPipelineAddUpdate(ClientPipelineDto input)
        {
            try
            {
                var existing = await _context.ClientPipelines
                    .FirstOrDefaultAsync(x => x.Id == input.Id && x.TenantId == _session.TenantId);

                var mapped = _mapper.Map<ClientPipeline>(input);
                mapped.TenantId = _session.TenantId;

                if (existing != null)
                {
                    _mapper.Map(input, existing);
                    _context.ClientPipelines.Update(existing);
                    await _context.SaveChangesAsync();
                    return OutputHandler.Handler((int)ResponseType.UPDATE, true, "Client Pipeline");
                }
                else
                {
                    _context.ClientPipelines.Add(mapped);
                    await _context.SaveChangesAsync();
                    return OutputHandler.Handler((int)ResponseType.CREATE, true, "Client Pipeline");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<OutputDTO<bool>> ClientPipelineDelete(int id)
        {
            try
            {
                var pipeline = await _context.ClientPipelines
                    .FirstOrDefaultAsync(x => x.Id == id && x.TenantId == _session.TenantId);

                if (pipeline == null)
                    return OutputHandler.Handler((int)ResponseType.NOT_EXIST, false, "Client Pipeline");

                _context.ClientPipelines.Remove(pipeline);
                await _context.SaveChangesAsync();
                return OutputHandler.Handler((int)ResponseType.DELETE, true, "Client Pipeline");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<OutputDTO<List<ClientPipelineDto>>> ClientPipelinesListGet()
        {
            try
            {
                var pipelines = await _context.ClientPipelines
                    .Where(x => x.TenantId == _session.TenantId)
                    .OrderBy(x => x.Id)
                    .ToListAsync();

                var mapped = _mapper.Map<List<ClientPipelineDto>>(pipelines);
                return OutputHandler.Handler((int)ResponseType.GET, mapped, "Client Pipelines", mapped.Count);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<OutputDTO<ClientPipelineDto>> ClientPipelineGet(int id)
        {
            try
            {
                var pipeline = await _context.ClientPipelines
                    .FirstOrDefaultAsync(x => x.Id == id && x.TenantId == _session.TenantId);

                if (pipeline == null)
                    return OutputHandler.Handler<ClientPipelineDto>((int)ResponseType.NOT_EXIST, null, "Client Manager");

                var mapped = _mapper.Map<ClientPipelineDto>(pipeline);
                return OutputHandler.Handler((int)ResponseType.GET, mapped, "Client Pipeline");
            }
            catch (Exception)
            {
                throw;
            }
        }


        #endregion


    }

}
