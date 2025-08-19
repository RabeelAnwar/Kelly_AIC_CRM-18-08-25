using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Audit;

namespace DataAccess.Entities
{
    public class SupportTicket : FullAuditedEntity, IMustHaveTenant
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public string Subject { get; set; }
        public string? Description { get; set; }
        public string? Document { get; set; }
    }
}
