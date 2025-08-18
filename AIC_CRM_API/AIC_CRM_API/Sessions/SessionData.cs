
namespace AIC_CRM_API.Sessions
{
    public class SessionData
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public SessionData(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public void SetSessionData()
        {
            _contextAccessor.HttpContext.Session.SetString("UserId", "1");
            _contextAccessor.HttpContext.Session.SetString("UserName", "Test");
            _contextAccessor.HttpContext.Session.SetString("UserRoleId", "1");
            _contextAccessor.HttpContext.Session.SetString("UserRole", "Admin");
            _contextAccessor.HttpContext.Session.SetString("CompanyId", "1");
            _contextAccessor.HttpContext.Session.SetString("CompanyName", "Test");
            _contextAccessor.HttpContext.Session.SetString("TenantId", "1001");
        }

        public SessionModel GetSessionData()
        {
            SessionModel? session = new SessionModel();
            session.UserId = Convert.ToInt32(_contextAccessor.HttpContext.Session.GetString("UserId"));
            session.UserName = _contextAccessor.HttpContext.Session.GetString("UserName");
            session.UserRoleId = Convert.ToInt32(_contextAccessor.HttpContext.Session.GetString("UserRoleId"));
            session.UserRole = _contextAccessor.HttpContext.Session.GetString("UserRole");
            session.CompanyId = Convert.ToInt32(_contextAccessor.HttpContext.Session.GetString("CompanyId"));
            session.CompanyName = _contextAccessor.HttpContext.Session.GetString("CompanyName");
            session.TenantId = Convert.ToInt32(_contextAccessor.HttpContext.Session.GetString("TenantId"));

            return session;
        }

    }


    public class SessionModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int UserRoleId { get; set; }
        public string UserRole { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int TenantId { get; set; }
    }
}
