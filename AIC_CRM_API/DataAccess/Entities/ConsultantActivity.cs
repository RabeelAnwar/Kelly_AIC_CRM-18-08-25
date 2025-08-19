using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Audit;

namespace DataAccess.Entities
{
    public class ConsultantActivity : FullAuditedEntity, IMustHaveTenant
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public int ClientId { get; set; }
        public int ManagerId { get; set; }
        public int ConsultantId { get; set; }
        public int RequisitionId { get; set; }
        public string AssignedToId { get; set; }
        public decimal? BillRate { get; set; }
        public decimal? PayRate { get; set; }
    }
}
