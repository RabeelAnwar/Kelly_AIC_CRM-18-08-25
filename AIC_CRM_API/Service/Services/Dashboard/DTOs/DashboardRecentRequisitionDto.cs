using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Dashboard.DTOs
{
    public class DashboardRecentRequisitionDto
    {
        public int Id { get; set; }
        public string Requisition { get; set; }
        public string? Type{ get; set; }
        public string? ClientName{ get; set; }
        public string? ManagerName{ get; set; }
        public string? Buzzwords{ get; set; }
        public string? Duration{ get; set; }
        public string? BillRate{ get; set; }
        public DateTime? CreatedDatetime{ get; set; }
    }
}
