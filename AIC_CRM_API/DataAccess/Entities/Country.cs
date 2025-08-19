using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Audit;

namespace DataAccess.Entities
{
    public class Country : FullAuditedEntity, IMustHaveTenant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TenantId { get; set; }

    }
}
