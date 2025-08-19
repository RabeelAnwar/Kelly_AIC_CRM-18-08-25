using System.ComponentModel.DataAnnotations;
using DataAccess.Audit;

namespace DataAccess.Entities
{
    public class User : IdentityUser, IMustHaveTenant, IFullAuditedEntity
    {
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string UserPassword { get; set; }
        public int ContactTypeId { get; set; }
        public virtual ContactType? ContactType { get; set; }

        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string? ZipCode { get; set; }

        public string? Phone1 { get; set; }
        public string? Phone1Ext { get; set; }
        public string? Phone2 { get; set; }
        public string? Phone2Ext { get; set; }
        public string? AlternatePhone { get; set; }
        public string? AlternatePhoneExt { get; set; }

        public string? WorkEmail { get; set; }
        public string? PersonalEmail { get; set; }
        public string? SkypeId { get; set; }

        public bool? ActiveStatus { get; set; }

        public int TenantId { get; set; }

        // Tracking Creation
        public string CreatorUserId { get; set; }

        public DateTime CreationTime { get; set; } = DateTime.UtcNow;

        // Tracking Modification
        public string? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }

        // Tracking Deletion
        public bool IsDeleted { get; set; }
        public string? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }

    }

}
