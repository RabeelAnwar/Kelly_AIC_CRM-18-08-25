using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Audit;

namespace DataAccess.Entities
{
    public class ConsultantInterview : FullAuditedEntity, IMustHaveTenant
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public int ConsultantId { get; set; }
        public string? Requisition { get; set; }
        public string? InterviewByStaff { get; set; }
        public string? InterviewByNonStaff { get; set; }
        public string? Comments { get; set; }
        public string? InterviewResults { get; set; }
    }
}
