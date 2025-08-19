using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Audit
{
    public class FullAuditedEntity : IFullAuditedEntity
    {
        // Tracking Creation
        public string CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }

        // Tracking Modification
        public string? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }

        // Tracking Deletion
        public bool IsDeleted { get; set; } = false;
        public string? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
    }


    public interface IFullAuditedEntity
    {
        // Tracking Creation
        public string CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; }

        // Tracking Modification
        public string? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }

        // Tracking Deletion
        public bool IsDeleted { get; set; }
        public string? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
    }


    public interface IMustHaveTenant
    {
        //
        // Summary:
        //     TenantId of this entity.
        public int TenantId { get; set; }
    }
}
