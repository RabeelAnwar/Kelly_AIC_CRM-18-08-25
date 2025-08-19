using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Dashboard.DTOs
{
    public class DashboardLeadsDto
    {
        public int Id { get; set; }
        public string? ClientName { get; set; }
        public DateTime? Date { get; set; }
        public string? ManagerName { get; set; }
        public string? ManagerContact { get; set; }
        public decimal? Amount { get; set; }
        public string? Details { get; set; }
        public string? Called { get; set; }
    }
}
