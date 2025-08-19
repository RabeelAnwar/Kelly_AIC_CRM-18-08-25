using DataAccess.Audit;

namespace DataAccess.Entities
{
    public class UserPermissions : FullAuditedEntity, IMustHaveTenant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanView { get; set; }
        public int TenantId { get; set; }
        public int UserId { get; set; }
        public virtual User Users { get; set; }
        public int RoleId { get; set; }
        public virtual Role Roles { get; set; }
    }
}
