
using System.Text;
using System.Text.Json;
using DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Service.Extensions;

namespace Service.Sessions
{

    public interface ISessionData
    {
        //void SetSessionData(SessionModel? obj);
        //SessionModel GetSessionData();

        int TenantId { get; }
        string UserId { get; }
    }


    public class SessionData : ISessionData
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionData(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int TenantId
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;
                int tenantId = user?.GetTenantIdAsInt() ?? 0;
                return tenantId == 0 ? 1001 : tenantId;
            }
        }
        public string UserId => _httpContextAccessor.HttpContext?.User?.GetUserId() ?? "system";


        //private readonly IHttpContextAccessor _contextAccessor;

        //public SessionData(IHttpContextAccessor contextAccessor)
        //{
        //    _contextAccessor = contextAccessor;
        //}

        //    public void SetSessionData(SessionModel session)
        //    {
        //        string sessionJson = JsonSerializer.Serialize(session);
        //        byte[] sessionBytes = Encoding.UTF8.GetBytes(sessionJson);

        //        var context = _contextAccessor?.HttpContext;
        //        if (context == null || context.Session == null) { }
        //        else
        //        {
        //            context.Session.Set("SessionData", sessionBytes);
        //            context.Session.CommitAsync().GetAwaiter().GetResult();
        //        }
        //    }

        //    public SessionModel GetSessionData()
        //    {
        //        var context = _contextAccessor?.HttpContext;
        //        if (context == null || context.Session == null)
        //            return new SessionModel();

        //        var userBytes = context.Session.Get("SessionData");

        //        if (userBytes != null)
        //        {
        //            string userJson = Encoding.UTF8.GetString(userBytes);

        //            var user = JsonSerializer.Deserialize<SessionModel>(userJson);
        //            return user;
        //        }
        //        return new SessionModel();
        //    }

    }


    public class SessionModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserRoleId { get; set; }
        public string UserRole { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int TenantId { get; set; }
    }
}
