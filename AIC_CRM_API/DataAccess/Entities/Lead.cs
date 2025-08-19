using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Audit;

namespace DataAccess.Entities
{
    public class Lead : FullAuditedEntity, IMustHaveTenant
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public int ClientId { get; set; }
        public string Category { get; set; }
        public string LeadType { get; set; }
        public string StatusOfLead { get; set; }
        public int DepartmentId { get; set; }
        public int ManagerId { get; set; }
        public string AssignedToId { get; set; }
        public string? GeneratedById { get; set; }
        public bool? Called { get; set; }
        public bool? IsStaffConsultant { get; set; }
        public bool? IsClientManager { get; set; }
        public string? ReferredBy { get; set; }
        public decimal? ApproximateAmount { get; set; }
        public string? Source { get; set; }
        public string? LeadInformation { get; set; }
        public string? Result { get; set; }
        public DateTime? ReminderDateTime { get; set; }
    }
}
