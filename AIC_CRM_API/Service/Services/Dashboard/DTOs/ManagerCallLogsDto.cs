using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Dashboard.DTOs
{
    public class DashboardCallLogsDto
    {
        public int? Id { get; set; }
        public DateTime? Datetime { get; set; }
        public string? ManagerName { get; set; }
        public string? ConsultantName { get; set; }
        public string? SpokeWith { get; set; }
        public string? CallRecord { get; set; }
    }
}
