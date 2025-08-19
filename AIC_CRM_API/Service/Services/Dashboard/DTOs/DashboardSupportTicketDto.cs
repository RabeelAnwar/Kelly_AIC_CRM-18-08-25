using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Dashboard.DTOs
{
    public class DashboardSupportTicketDto
    {

        public int Id { get; set; }
        public DateTime Datetime { get; set; }
        public string? Type { get; set; }
        public string? Status { get; set; }
        public string? Priority { get; set; }
        public string? Subject { get; set; }
        public string? Description { get; set; }
        public string? Document { get; set; }
        public string? RaisedBy { get; set; }
    }
}
