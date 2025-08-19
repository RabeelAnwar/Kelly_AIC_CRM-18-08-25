using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Audit;

namespace DataAccess.Entities
{
    public class UsersLog : FullAuditedEntity, IMustHaveTenant
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public string Action { get; set; }
        public string FormName { get; set; }
        public string FormId { get; set; }
        public string Message { get; set; }
    }
}
