
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Service.Services.UserLogs.DTOs;
using Service.Sessions;

namespace Service.Services.UserLogs
{
    //public class UserLogsService : IUserLogsService
    //{
    //    private readonly MainContext _context;
    //    private readonly IMapper _mapper;
    //    private readonly ISessionData _session;

    //    public UserLogsService(MainContext context, IMapper mapper, ISessionData sessionData)
    //    {
    //        _context = context;
    //        _mapper = mapper;
    //        _session = sessionData;
    //    }


    //    public async void SaveUserLogs(UsersLogDto input)
    //    {

    //        try
    //        {
    //            var mappedClient = _mapper.Map<UsersLog>(input);
    //            mappedClient.TenantId = _session.TenantId;

    //            _context.UsersLogs.Add(mappedClient);
    //            await _context.SaveChangesAsync();

    //        }
    //        catch (Exception)
    //        {
    //            throw;
    //        }
    //    }

    //}


    public static class UserLogExtensions
    {
        public static async Task SaveUserLogAsync(this DbContext context, UsersLogDto input, ISessionData session, IMapper mapper)
        {
            if (input == null || session == null || mapper == null)
                throw new ArgumentNullException("LogDto, session or mapper is null.");

            var logEntity = mapper.Map<UsersLog>(input);
            logEntity.TenantId = session.TenantId;

            if (context is MainContext mainContext)
            {
                mainContext.UsersLogs.Add(logEntity);
                await mainContext.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("DbContext is not of type MainContext.");
            }
        }
    }
}
