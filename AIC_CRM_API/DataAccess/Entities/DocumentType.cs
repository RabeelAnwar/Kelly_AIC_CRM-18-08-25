using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Audit;

namespace DataAccess.Entities
{
    public class DocumentType : FullAuditedEntity, IMustHaveTenant
    {
        public int Id { get; set; }
        public string DocumentTypeName { get; set; }
        public int UserTypeId { get; set; }
        public int TenantId { get; set; }
    }
}
