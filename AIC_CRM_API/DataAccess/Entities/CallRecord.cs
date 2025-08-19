using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Audit;

namespace DataAccess.Entities
{
    public class CallRecord : FullAuditedEntity, IMustHaveTenant
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public int? ClientId { get; set; }
        public int? LeadId { get; set; }
        public int? ManagerId { get; set; }
        public int? ConsultantId { get; set; }
        public DateTime Date { get; set; }
        public int? TypeId { get; set; }
        public string? Record { get; set; }
        public bool? RemindStatus { get; set; }
        public DateTime? ReminderDate { get; set; }
    }
}
