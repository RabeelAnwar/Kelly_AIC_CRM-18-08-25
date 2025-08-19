using System.Security.Claims;

namespace Service.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user?.FindFirst("UserId")?.Value;
        }
        public static string GetUserRoleId(this ClaimsPrincipal user)
        {
            return user?.FindFirst("UserRoleId")?.Value;
        }

        public static string GetCompanyName(this ClaimsPrincipal user)
        {
            return user?.FindFirst("CompanyName")?.Value;
        }

        public static int? GetCompanyIdAsInt(this ClaimsPrincipal user)
        {
            var companyIdValue = user?.FindFirst("CompanyId")?.Value;
            if (int.TryParse(companyIdValue, out var companyId))
            {
                return companyId;
            }
            return null;
        }

        //public static string GetTenantId(this ClaimsPrincipal user)
        //{
        //    return user?.FindFirst("TenantId")?.Value;
        //}

        public static int? GetTenantIdAsInt(this ClaimsPrincipal user)
        {
            var tenantIdValue = user?.FindFirst("TenantId")?.Value;
            if (int.TryParse(tenantIdValue, out var tenantId))
            {
                return tenantId;
            }
            return null;
        }

        public static string GetUserName(this ClaimsPrincipal user)
        {
            return user?.FindFirst(ClaimTypes.Name)?.Value;
        }

        public static string GetUserRole(this ClaimsPrincipal user)
        {
            return user?.FindFirst(ClaimTypes.Role)?.Value;
        }

    }
}
