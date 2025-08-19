using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Audit;

namespace DataAccess.Entities
{
    public class ConsultantReference : FullAuditedEntity, IMustHaveTenant
    {
        public int Id { get; set; }
        public int ConsultantId { get; set; }
        public int TenantId { get; set; }
        public string? ReferenceName { get; set; }
        public string? Company { get; set; }
        public string? ManagerPhone { get; set; }
        public string? ManagerEmail { get; set; }
        public DateTime? ReferenceDate { get; set; } = DateTime.Now;
        public string? EmployementDates { get; set; }
        public string? HiringManagerComments { get; set; }
    }
}
